using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using DeepLinkR.Core.Helper.SyncCommand;
using DeepLinkR.Ui.Events;

namespace DeepLinkR.Ui.ViewModels
{
	public class DeepLinkHistoryViewModel : Screen, IHandle<DeepLinkMatchesUpdatedEvent>
	{
		private IEventAggregator eventAggregator;

		public DeepLinkHistoryViewModel(IEventAggregator eventAggregator)
		{
			this.eventAggregator = eventAggregator;

			this.eventAggregator.Subscribe(this);
		}

		public ICommand TestCommand => new SyncCommand(() => this.OnTest());

		public void Handle(DeepLinkMatchesUpdatedEvent message)
		{
			// throw new NotImplementedException();
		}

		private void OnTest()
		{
			this.eventAggregator.PublishOnUIThread(new ErrorEvent(new Exception(string.Empty), "This is an error"));
		}

		private void OnHistoricalEntrySelected()
		{
			// this.eventAggregator.PublishOnUIThread(new DeepLinkMatchesUpdatedEvent(deepLinkMatches));
		}
	}
}
