using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types;

namespace DeepLinkR.Ui.Models
{
	public class HistoryEntry
	{
		public string DeepLinkMatchValue { get; set; }

		public List<DeepLinkMatch> DeepLinkMatches { get; set; }

		public string MatchingKeys
		{
			get
			{
				var uniqueKeyList = this.DeepLinkMatches.Select(x => x.DeepLinkKeyName).Distinct();
				var keys = string.Join(", ", uniqueKeyList);
				if (keys.Length > 30)
				{
					keys = keys.Substring(0, 30) + " ...";
				}

				return keys;
			}
		}
	}
}
