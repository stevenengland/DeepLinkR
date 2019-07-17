using System.Collections.Generic;
using DeepLinkR.Core.Types;

namespace DeepLinkR.Ui.Events
{
	public class DeepLinkMatchesUpdatedEvent
	{
		public DeepLinkMatchesUpdatedEvent(List<DeepLinkMatch> deepLinkMatches)
		{
			this.DeepLinkMatches = deepLinkMatches;
		}

		public List<DeepLinkMatch> DeepLinkMatches { get; private set; }
	}
}
