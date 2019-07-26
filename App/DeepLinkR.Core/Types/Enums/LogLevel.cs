using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DeepLinkR.Core.Types.Enums
{
	public enum LogLevel
	{
		[EnumMember(Value = "info")]
		Info,

		[EnumMember(Value = "trace")]
		Trace,

		[EnumMember(Value = "warn")]
		Warn,

		[EnumMember(Value = "debug")]
		Debug,

		[EnumMember(Value = "error")]
		Error,

		[EnumMember(Value = "fatal")]
		Fatal,
	}
}
