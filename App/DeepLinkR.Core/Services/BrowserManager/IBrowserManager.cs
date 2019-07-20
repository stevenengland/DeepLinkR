using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepLinkR.Core.Services.BrowserManager
{
	public interface IBrowserManager
	{
		Task<bool> OpenWithDefaultBrowserAsync(string url);
	}
}
