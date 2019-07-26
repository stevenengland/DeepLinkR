using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types;

namespace DeepLinkR.Core.Configuration
{
	public interface IDeepLinkConfiguration
	{
		List<DeepLinkCategory> DeepLinkCategories { get; }

		List<DeepLinkKey> DeepLinkKeys { get; }
	}
}
