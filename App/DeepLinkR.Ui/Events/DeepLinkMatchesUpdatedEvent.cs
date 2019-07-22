using System.Collections.Generic;
using DeepLinkR.Core.Types;

namespace DeepLinkR.Ui.Events
{
	public class DeepLinkMatchesUpdatedEvent
	{
		public DeepLinkMatchesUpdatedEvent(List<DeepLinkMatch> deepLinkMatches, string deepLinkMatchValue)
		{
			this.DeepLinkMatches = deepLinkMatches;
			this.DeepLinkMatchValue = deepLinkMatchValue;
		}

		public List<DeepLinkMatch> DeepLinkMatches { get; private set; }

		public string DeepLinkMatchValue { get; private set; }
	}
}
