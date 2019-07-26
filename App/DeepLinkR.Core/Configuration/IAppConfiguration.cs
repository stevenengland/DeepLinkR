using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepLinkR.Core.Configuration
{
	public interface IAppConfiguration
	{
		IBrowserConfiguration BrowserConfiguration { get; }

		IClipboardConfiguration ClipboardConfiguration { get; }

		ILoggingConfiguration LoggingConfiguration { get; }
	}
}
