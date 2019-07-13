using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepLinkR.Core.Types.EventArgs
{
	public class ClipboardTextUpdateEventArgs : System.EventArgs
	{
		public ClipboardTextUpdateEventArgs(string clipboardText)
		{
			this.ClipboardText = clipboardText;
		}

		public string ClipboardText { get; }
	}
}
