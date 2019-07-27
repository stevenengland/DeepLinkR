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

			foreach (var deepLinkKeyName in matchingKeyNames)
			{
				foreach (var deepLinkCategory in this.configuration.DeepLinkCategories)
				{
					foreach (var deepLink in deepLinkCategory.DeepLinks)
					{
						var match = Regex.Match(deepLink.Url, "{(" + deepLinkKeyName[0] + ")[|]?(.*)}");
						if (match.Success)
						{
							if (string.IsNullOrEmpty(match.Groups[2].Value))
							{
								resultSet.Add(new DeepLinkMatch()
								{
									DeepLinkKeyName = deepLinkKeyName[0],
									DeepLinkCategoryName = deepLinkCategory.Name,
									DeepLinkName = deepLink.Name,
									DeepLinkUrl = deepLink.Url.Replace(match.Groups[0].Value, text),
								});
							}
							else
							{
								var regex = new Regex(deepLinkKeyName[1]);
								resultSet.Add(new DeepLinkMatch()
								{
									DeepLinkKeyName = deepLinkKeyName[0],
									DeepLinkCategoryName = deepLinkCategory.Name,
									DeepLinkName = deepLink.Name,
									DeepLinkUrl = deepLink.Url.Replace(
										match.Groups[0].Value,
										regex.Replace(text, match.Groups[2].Value)),
								});
							}
						}
					}
				}
			}

			return resultSet;
		}

		public List<string[]> GetMatchingKeyNames(string text)
		{
			var matchingKeyNames = new List<string[]>();

			foreach (var deepLinkKey in this.configuration.DeepLinkKeys)
			{
				foreach (var regex in deepLinkKey.RegExPattern)
				{
					var match = Regex.Match(text, regex, RegexOptions.Compiled);
					if (match.Success)
					{
						var collection = new[] { deepLinkKey.Name, regex };

						matchingKeyNames.Add(collection);
						break;
					}
				}
			}

			return matchingKeyNames;
		}
	}
}
