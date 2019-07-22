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
using DeepLinkR.Core.Services.BrowserManager;
using DeepLinkR.Core.Services.ClipboardManager;
using DeepLinkR.Core.Services.DeepLinkManager;
using DeepLinkR.Core.Services.ProcessProxy;
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
		private IBrowserManager browserManager;
		private string currentMatchName;

		public DeepLinkCollectionViewModel(
			IConfigurationCollection configurationCollection,
			IClipboardManager clipboardManager,
			IDeepLinkManager deepLinkManager,
			IMapper mapper,
			IEventAggregator eventAggregator,
			IBrowserManager browserManager)
		{
			this.clipboardManager = clipboardManager;
			this.configurationCollection = configurationCollection;
			this.deepLinkManager = deepLinkManager;
			this.mapper = mapper;
			this.eventAggregator = eventAggregator;
			this.browserManager = browserManager;

			this.clipboardManager.ClipboardTextUpdateReceived += this.OnClipboardTextUpdateReceived;
			this.eventAggregator.Subscribe(this);

			this.currentMatchName = "-";
		}

		public ICommand ChangeSortOrderDirectionCommand => new SyncCommand(() => this.OnChangeSortOrderDirection());

		public ICommand ChangeDeepLinkSortOrderDirectionCommand => new SyncCommand(() => this.OnChangeDeepLinkSortOrderDirection());

		public ICommand CopyLinkToClipboardCommand => new SyncCommand<string>((arg) => this.OnCopyLinkToClipboard(arg));

		public ICommand OpenWithDefaultBrowserCommand => new SyncCommand<string>((arg) => this.OnOpenWithDefaultBrowser(arg));

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

		public string CurrentMatchName
		{
			get => this.currentMatchName;
			set
			{
				this.currentMatchName = value;
				this.NotifyOfPropertyChange(() => this.CurrentMatchName);
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

					var groupedDeepLinksByCategory = descendingSortOrder
						? deepLinkMatches.GroupBy(match => new { match.DeepLinkCategoryName, match.DeepLinkKeyName, })
								.OrderByDescending(matches => matches.Key.DeepLinkCategoryName)
								.ThenByDescending(matches => matches.Key.DeepLinkKeyName)
						: deepLinkMatches.GroupBy(match => new { match.DeepLinkCategoryName, match.DeepLinkKeyName, })
								.OrderBy(matches => matches.Key.DeepLinkCategoryName)
								.ThenBy(matches => matches.Key.DeepLinkKeyName)
						;

					foreach (var group in groupedDeepLinksByCategory)
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
					var groupedDeepLinksByKey = descendingSortOrder
							? deepLinkMatches.GroupBy(match => new { match.DeepLinkKeyName, match.DeepLinkCategoryName, })
								.OrderByDescending(matches => matches.Key.DeepLinkKeyName)
								.ThenByDescending(matches => matches.Key.DeepLinkCategoryName)
							: deepLinkMatches.GroupBy(match => new { match.DeepLinkKeyName, match.DeepLinkCategoryName, })
								.OrderBy(matches => matches.Key.DeepLinkKeyName)
								.ThenBy(matches => matches.Key.DeepLinkCategoryName)
						;

					foreach (var group in groupedDeepLinksByKey)
					{
						if (levelOneList.All(x => x.Name != @group.Key.DeepLinkKeyName))
						{
							levelOneList.Add(new HierarchyLevelOne() { Name = group.Key.DeepLinkKeyName, SecondLinkHierarchies = new List<HierarchyLevelTwo>() });
						}

						var levelOneItem = levelOneList.First(x => x.Name == group.Key.DeepLinkKeyName);
						if (levelOneItem.SecondLinkHierarchies.Count(x => x.Name == @group.Key.DeepLinkCategoryName) == 0)
						{
							levelOneItem.SecondLinkHierarchies.Add(new HierarchyLevelTwo() { Name = group.Key.DeepLinkCategoryName, ThirdLinkHierarchies = new List<HierarchyLevelThree>() });
						}

						var levelTwoItem =
							levelOneItem.SecondLinkHierarchies.First(x => x.Name == group.Key.DeepLinkCategoryName);
						foreach (var item in group)
						{
							levelTwoItem.ThirdLinkHierarchies.Add(new HierarchyLevelThree() { Name = item.DeepLinkName, Url = item.DeepLinkUrl });
						}
					}

					break;
				default:
					break;
			}

			return levelOneList;
		}

		private void OnClipboardTextUpdateReceived(object sender, ClipboardTextUpdateEventArgs e)
		{
			var clipboardEntries = e.ClipboardEntries;
			for (int i = clipboardEntries.Length - 1; i >= 0; i--)
			{
				var deepLinkMatches = this.deepLinkManager.GetDeepLinkMatches(clipboardEntries[i]);
				if (deepLinkMatches?.Count > 0)
				{
					if (i == 0)
					{
						this.CurrentMatchName = clipboardEntries[i];
						this.Sideload(deepLinkMatches);
					}

					this.eventAggregator.PublishOnUIThread(new DeepLinkMatchesUpdatedEvent(deepLinkMatches));
				}
			}
		}

		private void OnChangeSortOrderDirection()
		{
			this.isDescendingSortOrder = !this.isDescendingSortOrder;
			this.Sideload(this.deepLinkMatches, this.deepLinkSortOrder, this.isDescendingSortOrder);
		}

		private void OnChangeDeepLinkSortOrderDirection()
		{
			this.deepLinkSortOrder = this.deepLinkSortOrder == DeepLinkSortOrder.Category
				? DeepLinkSortOrder.Key
				: DeepLinkSortOrder.Category;
			this.Sideload(this.deepLinkMatches, this.deepLinkSortOrder, this.isDescendingSortOrder);
		}

		private void OnCopyLinkToClipboard(string text)
		{
			try
			{
				this.clipboardManager.CopyTextToClipboard(text);
			}
			catch (Exception e)
			{
				this.eventAggregator.PublishOnUIThread(new ErrorEvent(e, "Couldn't copy to clipboard :( Please try again."));
			}
		}

		private void OnOpenWithDefaultBrowser(string url)
		{
			// ToDo: TryCatch
			this.browserManager.OpenWithDefaultBrowser(url);
		}
	}
}
