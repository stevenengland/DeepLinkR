using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepLinkR.Core.Helper.LibraryMapper.TextCopyMapper
{
	public class TextCopyMapper : ITextCopyMapper
	{
		public void SetText(string text)
		{
			TextCopy.Clipboard.SetText(text);
		}
	}
}
