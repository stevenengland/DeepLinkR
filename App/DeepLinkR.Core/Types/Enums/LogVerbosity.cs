using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DeepLinkR.Core.Types.Enums
{
	public enum LogVerbosity
	{
		[EnumMember(Value = "quit")]
		Quit,

		[EnumMember(Value = "minimal")]
		Minimal,

		[EnumMember(Value = "normal")]
		Normal,

		[EnumMember(Value = "verbose")]
		Verbose,

		[EnumMember(Value = "diagnostic")]
		Diagnostic,
	}
}
