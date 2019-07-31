using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types.EventArgs;
using WK.Libraries.SharpClipboardNS;

namespace DeepLinkR.Core.Helper.LibraryMapper.SharpClipboardMapper
{
	public class SharpClipboardMapper : ISharpClipboardMapper
	{
		private SharpClipboard sharpClipboard = new SharpClipboard();

		public SharpClipboardMapper()
		{
			this.sharpClipboard.ClipboardChanged += this.OnSharpClipboardChanged;
		}

		public event EventHandler<ClipboardChangedEventArgs> ClipboardChanged;

		public string ClipboardText
		{
			get => this.sharpClipboard.ClipboardText;
		}

		private void OnSharpClipboardChanged(object sender, SharpClipboard.ClipboardChangedEventArgs e)
		{
			this.ClipboardChanged?.Invoke(this, new ClipboardChangedEventArgs(e.SourceApplication.Name, e.SourceApplication.Title, e.Content, e.ContentType));
		}
	}
}
