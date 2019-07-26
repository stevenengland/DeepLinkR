using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using DeepLinkR.Core.Helper.SyncCommand;
using DeepLinkR.Ui.Events;
using DeepLinkR.Ui.Models;

namespace DeepLinkR.Ui.ViewModels
{
	public class DeepLinkHistoryViewModel : Screen, IHandle<DeepLinkMatchesUpdatedEvent>
	{
		private IEventAggregator eventAggregator;

		private BindingList<HistoryEntry> historyEntryList;

		private HistoryEntry historyEntry;

		public DeepLinkHistoryViewModel(IEventAggregator eventAggregator)
		{
			this.HistoryEntryList = new BindingList<HistoryEntry>();
			this.eventAggregator = eventAggregator;

			this.eventAggregator.Subscribe(this);
		}

		public ICommand HistoryItemsSelectionChangedCommand => new SyncCommand<object>((arg) => this.OnHistoryItemsSelectionChanged(arg));

		public HistoryEntry HistoryEntry
		{
			get => this.historyEntry;
			set
			{
				this.historyEntry = value;
				this.NotifyOfPropertyChange(() => this.HistoryEntry);
			}
		}

		public BindingList<HistoryEntry> HistoryEntryList
		{
			get => this.historyEntryList;
			set
			{
				this.historyEntryList = value;
				this.NotifyOfPropertyChange(() => this.HistoryEntryList);
			}
		}

		public void Handle(DeepLinkMatchesUpdatedEvent message)
		{
			if (this.HistoryEntryList.Any(x => x.DeepLinkMatchValue == message.DeepLinkMatchValue))
			{
				return;
			}

			if (this.HistoryEntryList.Count >= 10)
			{
				this.HistoryEntryList.RemoveAt(this.HistoryEntryList.Count - 1);
			}

			this.HistoryEntryList.Insert(0, new HistoryEntry()
			{
				DeepLinkMatchValue = message.DeepLinkMatchValue,
				DeepLinkMatches = message.DeepLinkMatches,
			});
		}

		private void OnHistoryItemsSelectionChanged(object item)
		{
			this.HistoryEntry = (HistoryEntry)item;
			this.eventAggregator.PublishOnUIThread(new HistoricalDeepLinkSelectedEvent(this.HistoryEntry.DeepLinkMatches, this.HistoryEntry.DeepLinkMatchValue));
		}
	}
}
