using System;
using DeepLinkR.Ui.Events;
using NHotkey.Wpf;

namespace DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper
{
	public interface INHotkeyManagerMapper
	{
		event EventHandler<HotkeyAlreadyRegisteredEventArgs> HotkeyAlreadyRegistered;

		event EventHandler<HotKeyEventArgs> HotKeyPressed;

		void AddOrReplace(string name, System.Windows.Input.Key key, System.Windows.Input.ModifierKeys modifierKeys);
	}
}
