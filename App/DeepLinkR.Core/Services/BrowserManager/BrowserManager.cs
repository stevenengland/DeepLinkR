using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Services.ProcessProxy;
using DeepLinkR.Core.Types;

namespace DeepLinkR.Core.Services.BrowserManager
{
	public class BrowserManager : IBrowserManager
	{
		private IBrowserConfiguration browserConfiguration;
		private IProcessProxy processProxy;

		public BrowserManager(IBrowserConfiguration configuration, IProcessProxy processProxy)
		{
			this.browserConfiguration = configuration;
			this.processProxy = processProxy;

			this.DetectDefaultBrowserDefinition();
		}

		public BrowserDefinition DefaultBrowserDefinition { get; private set; }

		public string ConstructArguments(string arguments, string url)
		{
			var result = arguments ?? string.Empty;
			if (result.Contains("{url}"))
			{
				result = result.Replace("{url}", url);
			}
			else
			{
				result = result == string.Empty ? url : result + " " + url;
			}

			return result;
		}

		public void OpenUrl(BrowserDefinition browserDefinition, string url)
		{
			var startInfo = new ProcessStartInfo
			{
				Arguments = this.ConstructArguments(browserDefinition.CommandlineArguments, url),
				FileName = browserDefinition.PathToExecutable,
			};
			this.processProxy.StartProcess(startInfo);
		}

		public void OpenUrl(string browserDefinitionName, string url)
		{
			var browserDefinition =
				this.browserConfiguration.BrowserDefinitions.First(x => x.Name == browserDefinitionName);
			this.OpenUrl(browserDefinition, url);
		}

		public void OpenWithDefaultBrowser(string url)
		{
			this.OpenUrl(this.DefaultBrowserDefinition, url);
		}

		private void DetectDefaultBrowserDefinition()
		{
			this.DefaultBrowserDefinition =
				this.browserConfiguration.BrowserDefinitions.FirstOrDefault(x => x.IsDefault) ?? this.browserConfiguration.BrowserDefinitions.First();
		}
	}
}
