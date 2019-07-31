using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types.EventArgs;

namespace DeepLinkR.Core.Services.ClipboardManager
{
	public interface IClipboardManager
	{
		event EventHandler<ClipboardTextUpdateEventArgs> ClipboardTextUpdateReceived;

		void CopyTextToClipboard(string text);

		string AppIdentifier { get; set; }
	}
}
