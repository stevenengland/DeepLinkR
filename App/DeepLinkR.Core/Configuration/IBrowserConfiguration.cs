using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types;

namespace DeepLinkR.Core.Configuration
{
	public interface IBrowserConfiguration
	{
		List<BrowserDefinition> BrowserDefinitions { get; }
	}
}
