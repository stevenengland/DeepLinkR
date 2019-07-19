using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Caliburn.Micro;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Helper.SyncCommand;
using DeepLinkR.Core.Services.ClipboardManager;
using DeepLinkR.Core.Services.DeepLinkManager;
using DeepLinkR.Core.Types;
using DeepLinkR.Core.Types.Enums;
using DeepLinkR.Core.Types.EventArgs;
using DeepLinkR.Ui.Events;
using DeepLinkR.Ui.Models;

namespace DeepLinkR.Ui.ViewModels
{
	public class DeepLinkCollectionViewModel : Screen, IHandle<HistoricalDeepLinkSelectedEvent>
	{
		private IConfigurationCollection configurationCollection;
		private IClipboardManager clipboardManager;
		private IDeepLinkManager deepLinkManager;
		private IMapper mapper;
		private IEventAggregator eventAggregator;
		private BindingList<DeepLinkMatchDisplayModel> deepLinkMatchesDisplayModels;
		private BindingList<HierarchyLevelOne> hierarchicalLinks;
		private bool isDescendingSortOrder;
		private DeepLinkSortOrder deepLinkSortOrder;
		private List<DeepLinkMatch> deepLinkMatches;

		public DeepLinkCollectionViewModel(IConfigurationCollection configurationCollection, IClipboardManager clipboardManager, IDeepLinkManager deepLinkManager, IMapper mapper, IEventAggregator eventAggregator)
		{
			this.clipboardManager = clipboardManager;
			this.configurationCollection = configurationCollection;
			this.deepLinkManager = deepLinkManager;
			this.mapper = mapper;
			this.eventAggregator = eventAggregator;

			this.clipboardManager.ClipboardTextUpdateReceived += this.OnClipboardTextUpdateReceived;
			this.eventAggregator.Subscribe(this);
		}

		public ICommand ChangeSortOrderDirectionCommand => new SyncCommand(() => this.OnChangeSortOrderDirection());

		public BindingList<DeepLinkMatchDisplayModel> DeepLinkMatchesDisplayModels
		{
			get => this.deepLinkMatchesDisplayModels;
			set
			{
				this.deepLinkMatchesDisplayModels = value;
				this.NotifyOfPropertyChange(() => this.DeepLinkMatchesDisplayModels);
			}
		}

		public BindingList<HierarchyLevelOne> HierarchicalLinks
		{
			get => this.hierarchicalLinks;
			set
			{
				this.hierarchicalLinks = value;
				this.NotifyOfPropertyChange(() => this.HierarchicalLinks);
			}
		}

		public void Sideload(List<DeepLinkMatch> deepLinkMatches, DeepLinkSortOrder deepLinkSortOrder = DeepLinkSortOrder.Category, bool descendingSortOrder = false)
		{
			// this.DeepLinkMatchesDisplayModels = new BindingList<DeepLinkMatchDisplayModel>(this.mapper.Map<List<DeepLinkMatchDisplayModel>>(deepLinkMatches));

			// this.HierarchicalLinks = this.CreateSortedHierarchy(this.DeepLinkMatchesDisplayModels, deepLinkSortOrder.Category);
			this.deepLinkMatches = deepLinkMatches;
			this.HierarchicalLinks = new BindingList<HierarchyLevelOne>(this.CreateSortedHierarchy(this.deepLinkMatches, deepLinkSortOrder, descendingSortOrder));
		}

		public void Handle(HistoricalDeepLinkSelectedEvent message)
		{
			this.Sideload(message.DeepLinkMatches, this.deepLinkSortOrder, this.isDescendingSortOrder);
		}

		private List<HierarchyLevelOne> CreateSortedHierarchy(List<DeepLinkMatch> deepLinkMatches, DeepLinkSortOrder deepLinkSortOrder, bool descendingSortOrder)
		{
			var levelOneList = new List<HierarchyLevelOne>();

			switch (deepLinkSortOrder)
			{
				case DeepLinkSortOrder.Category:

					var groupedDeepLinks = descendingSortOrder
						? deepLinkMatches.GroupBy(match => new {match.DeepLinkCategoryName, match.DeepLinkKeyName,})
								.OrderByDescending(matches => matches.Key.DeepLinkCategoryName)
								.ThenByDescending(matches => matches.Key.DeepLinkKeyName)
						: deepLinkMatches.GroupBy(match => new {match.DeepLinkCategoryName, match.DeepLinkKeyName,})
								.OrderBy(matches => matches.Key.DeepLinkCategoryName)
								.ThenBy(matches => matches.Key.DeepLinkKeyName)
						;

					foreach (var group in groupedDeepLinks)
					{
						if (levelOneList.All(x => x.Name != @group.Key.DeepLinkCategoryName))
						{
							levelOneList.Add(new HierarchyLevelOne() { Name = group.Key.DeepLinkCategoryName, SecondLinkHierarchies = new List<HierarchyLevelTwo>() });
						}

						var levelOneItem = levelOneList.First(x => x.Name == group.Key.DeepLinkCategoryName);
						if (levelOneItem.SecondLinkHierarchies.Count(x => x.Name == @group.Key.DeepLinkKeyName) == 0)
						{
							levelOneItem.SecondLinkHierarchies.Add(new HierarchyLevelTwo() { Name = group.Key.DeepLinkKeyName, ThirdLinkHierarchies = new List<HierarchyLevelThree>() });
						}

						var levelTwoItem =
							levelOneItem.SecondLinkHierarchies.First(x => x.Name == group.Key.DeepLinkKeyName);
						foreach (var item in group)
						{
							levelTwoItem.ThirdLinkHierarchies.Add(new HierarchyLevelThree() { Name = item.DeepLinkName, Url = item.DeepLinkUrl });
						}
					}

					break;
				case DeepLinkSortOrder.Key:
					break;
				default:
					break;
			}

			return levelOneList;
		}

		private void OnClipboardTextUpdateReceived(object sender, ClipboardTextUpdateEventArgs e)
		{
			var deepLinkMatches = this.deepLinkManager.GetDeepLinkMatches(e.ClipboardText);
			if (deepLinkMatches?.Count > 0)
			{
				this.Sideload(deepLinkMatches);
				this.eventAggregator.PublishOnUIThread(new DeepLinkMatchesUpdatedEvent(deepLinkMatches));
			}
		}

		private void OnChangeSortOrderDirection()
		{
			this.isDescendingSortOrder = !this.isDescendingSortOrder;
			this.Sideload(this.deepLinkMatches, this.deepLinkSortOrder, this.isDescendingSortOrder);
		}
	}
}
