using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Types;
using DeepLinkR.Core.Types.EventArgs;

namespace DeepLinkR.Core.Services.DeepLinkManager
{
	public class DeepLinkManager : IDeepLinkManager
	{
		private IDeepLinkConfiguration configuration;

		public DeepLinkManager(IDeepLinkConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public List<DeepLinkMatch> GetDeepLinkMatches(string text)
		{
			var resultSet = new List<DeepLinkMatch>();

			var matchingKeyNames = this.GetMatchingKeyNames(text);

			foreach (var deepLingKeyName in matchingKeyNames)
			{
				foreach (var deepLinkCategory in this.configuration.DeepLinkCategories)
				{
					foreach (var deepLink in deepLinkCategory.DeepLinks)
					{
						if (deepLink.Url.Contains("{" + deepLingKeyName + "}"))
						{
							resultSet.Add(new DeepLinkMatch()
							{
								DeepLinkKeyName = deepLingKeyName,
								DeepLinkCategoryName = deepLinkCategory.Name,
								DeepLinkName = deepLink.Name,
								DeepLinkUrl = deepLink.Url.Replace("{" + deepLingKeyName + "}", text),
							});
						}
					}
				}
			}

			return resultSet;
		}

		public List<string> GetMatchingKeyNames(string text)
		{
			var matchingKeyNames = new List<string>();

			foreach (var deepLinkKey in this.configuration.DeepLinkKeys)
			{
				foreach (var regex in deepLinkKey.RegExPattern)
				{
					if (Regex.IsMatch(text, regex))
					{
						matchingKeyNames.Add(deepLinkKey.Name);
						break;
					}
				}
			}

			return matchingKeyNames;
		}
	}
}
