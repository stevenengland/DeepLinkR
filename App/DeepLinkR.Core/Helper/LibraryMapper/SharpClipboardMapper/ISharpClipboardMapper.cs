using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types.EventArgs;
using WK.Libraries.SharpClipboardNS;

namespace DeepLinkR.Core.Helper.LibraryMapper.SharpClipboardMapper
{
	public interface ISharpClipboardMapper
	{
		event EventHandler<ClipboardChangedEventArgs> ClipboardChanged;

		string ClipboardText { get; }
	}
}
