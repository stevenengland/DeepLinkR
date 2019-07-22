using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepLinkR.Core.Services.ProcessProxy
{
	public interface IProcessProxy
	{
		void StartProcess(ProcessStartInfo processStartInfo);
	}
}
