using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DeepLinkR.Core.Services.ClipboardManager;
using DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper;
using NHotkey;

namespace DeepLinkR.Ui.ViewModels
{
	public class ShellViewModel
	{
		private IClipboardManager clipboardManager;
		private INHotkeyManagerMapper hotkeyManager;

		public ShellViewModel(IClipboardManager clipboardManager, INHotkeyManagerMapper hotkeyManager)
		{
			this.clipboardManager = clipboardManager;
			this.hotkeyManager = hotkeyManager;

			// LoadConfiguration
			this.RegisterHotkey();
		}

		private void RegisterHotkey()
		{
			this.hotkeyManager.AddOrReplace("OpenDeepLinkR", Key.Space, ModifierKeys.Alt, this.OnDeepLinkROpen);
		}

		private void OnDeepLinkROpen(object sender, HotkeyEventArgs e)
		{
			// OpenMainWindow
		}
	}
}
