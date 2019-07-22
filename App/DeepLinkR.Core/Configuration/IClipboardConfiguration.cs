using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepLinkR.Core.Configuration
{
	public interface IClipboardConfiguration
	{
		bool AutomaticTrim { get; }

		bool ProcessMultipleRows { get; }
	}
}
