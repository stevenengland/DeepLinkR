using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WK.Libraries.SharpClipboardNS;

namespace DeepLinkR.Core.Types.EventArgs
{
	public class ClipboardChangedEventArgs : System.EventArgs
	{
		public ClipboardChangedEventArgs(string applicationName, string applicationTitle, object clipboardContent, SharpClipboard.ContentTypes contentType)
		{
			this.ApplicationName = applicationName;
			this.ApplicationTitle = applicationTitle;
			this.ClipboardContent = clipboardContent;
			this.ContentType = contentType;
		}

		public string ApplicationName { get; set; }

		public string ApplicationTitle { get; set; }

		public object ClipboardContent { get; set; }

		public SharpClipboard.ContentTypes ContentType { get; set; }
	}
}
