using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types.Enums;

namespace DeepLinkR.Core.Configuration
{
	public interface ILoggingConfiguration
	{
		bool IsLoggingEnabled { get; }

		LogLevel LogLevel { get; }

		LogVerbosity LogVerbosity { get; }
	}
}
