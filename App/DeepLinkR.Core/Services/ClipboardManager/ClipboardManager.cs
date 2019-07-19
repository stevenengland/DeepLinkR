using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DeepLinkR.Core.Types.EventArgs;
using WK.Libraries.SharpClipboardNS;

namespace DeepLinkR.Core.Services.ClipboardManager
{
	public class ClipboardManager : IClipboardManager
	{
		private SharpClipboard clipboard = new SharpClipboard();

		private string appName = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";

		public ClipboardManager()
		{
			this.clipboard.ClipboardChanged += this.ClipboardChanged;
		}

		public event EventHandler<ClipboardTextUpdateEventArgs> ClipboardTextUpdateReceived;

		public void CopyTextToClipboard(string text)
		{
			// https://stackoverflow.com/questions/68666/clipbrd-e-cant-open-error-when-setting-the-clipboard-from-net
			// Clipboard might deny using it, keep trying (WPF doesn't have a retry mechanism on its own) -> I use a 3rd party library for this
			TextCopy.Clipboard.SetText(text);
		}

		private void ClipboardChanged(object sender, SharpClipboard.ClipboardChangedEventArgs e)
		{
			// Is the content copied of text type?
			if (e.ContentType == SharpClipboard.ContentTypes.Text)
			{
				// Suppress monitoring events from the own application
				if (e.SourceApplication.Name != this.appName)
				{
					this.ClipboardTextUpdateReceived?.Invoke(this, new ClipboardTextUpdateEventArgs(this.clipboard.ClipboardText));
				}
			}
		}
	}
}
