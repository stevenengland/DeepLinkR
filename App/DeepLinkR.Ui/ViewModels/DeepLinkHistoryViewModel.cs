using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
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

		public void Handle(DeepLinkMatchesUpdatedEvent message)
		{
			// throw new NotImplementedException();
		}

		private void OnHistoricalEntrySelected()
		{
			// this.eventAggregator.PublishOnUIThread(new DeepLinkMatchesUpdatedEvent(deepLinkMatches));
		}
	}
}
