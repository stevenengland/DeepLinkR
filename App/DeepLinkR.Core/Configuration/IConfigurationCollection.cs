using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepLinkR.Core.Configuration
{
	public interface IConfigurationCollection
	{
		IAppConfiguration AppConfiguration { get; set; }

		IDeepLinkConfiguration DeepLinkConfiguration { get; set; }
	}
}
