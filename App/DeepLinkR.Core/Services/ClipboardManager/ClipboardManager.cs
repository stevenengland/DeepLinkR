using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Helper.LibraryMapper.SharpClipboardMapper;
using DeepLinkR.Core.Helper.LibraryMapper.TextCopyMapper;
using DeepLinkR.Core.Types.EventArgs;
using WK.Libraries.SharpClipboardNS;

namespace DeepLinkR.Core.Services.ClipboardManager
{
	public class ClipboardManager : IClipboardManager
	{
		private IClipboardConfiguration clipboardConfiguration;
		private ISharpClipboardMapper sharpClipboardMapper;
		private ITextCopyMapper textCopyMapper;
		private string appName = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";

		public ClipboardManager(IClipboardConfiguration clipboardConfiguration, ISharpClipboardMapper sharpClipboardMapper, ITextCopyMapper textCopyMapper)
		{
			this.clipboardConfiguration = clipboardConfiguration;
			this.sharpClipboardMapper = sharpClipboardMapper;
			this.textCopyMapper = textCopyMapper;

			this.sharpClipboardMapper.ClipboardChanged += this.OnClipboardChanged;
		}

		public event EventHandler<ClipboardTextUpdateEventArgs> ClipboardTextUpdateReceived;

		public string AppName
		{
			get => this.appName;
			set => this.appName = value;
		}

		public void CopyTextToClipboard(string text)
		{
			// https://stackoverflow.com/questions/68666/clipbrd-e-cant-open-error-when-setting-the-clipboard-from-net
			// Clipboard might deny using it, keep trying (WPF doesn't have a retry mechanism on its own) -> I use a 3rd party library for this
			this.textCopyMapper.SetText(text);
		}

		private void OnClipboardChanged(object sender, ClipboardChangedEventArgs e)
		{
			// Is the content copied of text type?
			if (e.ContentType == SharpClipboard.ContentTypes.Text)
			{
				var text = (string)e.ClipboardContent;

				// Suppress monitoring events from the own application
				if (e.ApplicationName != this.appName)
				{
					string[] entries;
					if (this.clipboardConfiguration.ProcessMultipleRows)
					{
						entries = text.Split(
							new[] { Environment.NewLine },
							StringSplitOptions.None);
					}
					else
					{
						entries = new string[] { text };
					}

					if (this.clipboardConfiguration.AutomaticTrim)
					{
						for (int i = 0; i < entries.Length; i++)
						{
							entries[i] = entries[i].Trim();
						}
					}

					this.ClipboardTextUpdateReceived?.Invoke(this, new ClipboardTextUpdateEventArgs(entries));
				}
			}
		}
	}
}
