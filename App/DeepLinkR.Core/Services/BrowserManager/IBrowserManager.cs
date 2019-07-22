using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types;

namespace DeepLinkR.Core.Services.BrowserManager
{
	public interface IBrowserManager
	{
		void OpenWithDefaultBrowser(string url);

		void OpenUrl(string browserDefinitionName, string url);

		void OpenUrl(BrowserDefinition browserDefinition, string url);
	}
}
