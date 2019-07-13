using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types;
using DeepLinkR.Core.Types.EventArgs;

namespace DeepLinkR.Core.Services.DeepLinkManager
{
	public interface IDeepLinkManager
	{
		List<DeepLinkMatch> GetDeepLinkMatches(string text);
	}
}
