using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types.EventArgs;
using WK.Libraries.SharpClipboardNS;

namespace DeepLinkR.Core.Services.ClipboardManager
{
	public class ClipboardManager : IClipboardManager
	{
		private SharpClipboard clipboard = new SharpClipboard();

		public ClipboardManager()
		{
			this.clipboard.ClipboardChanged += this.ClipboardChanged;
		}

		public event EventHandler<ClipboardTextUpdateEventArgs> ClipboardTextUpdateReceived;

		private void ClipboardChanged(object sender, SharpClipboard.ClipboardChangedEventArgs e)
		{
			// Is the content copied of text type?
			if (e.ContentType == SharpClipboard.ContentTypes.Text)
			{
				this.ClipboardTextUpdateReceived?.Invoke(this, new ClipboardTextUpdateEventArgs(this.clipboard.ClipboardText));
			}
		}
	}
}
