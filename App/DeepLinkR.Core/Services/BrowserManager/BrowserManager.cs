using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Types;

namespace DeepLinkR.Core.Services.BrowserManager
{
	public class BrowserManager : IBrowserManager
	{
		private IBrowserConfiguration browserConfiguration;
		private BrowserDefinition defaultBrowserDefinition;

		public BrowserManager(IBrowserConfiguration configuration)
		{
			this.browserConfiguration = configuration;

			this.DetectDefaultBrowserDefinition();
		}

		public async Task<bool> OpenWithDefaultBrowserAsync(string url)
		{
			throw new NotImplementedException();
		}

		private void DetectDefaultBrowserDefinition()
		{

		}
	}
}
