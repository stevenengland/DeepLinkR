using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types;

namespace DeepLinkR.Ui.Events
{
	public class HistoricalDeepLinkSelectedEvent
	{
		public HistoricalDeepLinkSelectedEvent(List<DeepLinkMatch> deepLinkMatches, string deepLinkMatchValue)
		{
			this.DeepLinkMatches = deepLinkMatches;
			this.DeepLinkMatchValue = deepLinkMatchValue;
		}

		public List<DeepLinkMatch> DeepLinkMatches { get; private set; }

		public string DeepLinkMatchValue { get; private set; }
	}
}
