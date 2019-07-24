using System;
using DeepLinkR.Ui.Events;
using NHotkey.Wpf;

namespace DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper
{
	public interface INHotkeyManagerMapper
	{
		event EventHandler<MappedHotkeyAlreadyRegisteredEventArgs> HotkeyAlreadyRegistered;

		event EventHandler<MappedHotKeyEventArgs> HotKeyPressed;

		void AddOrReplace(string name, System.Windows.Input.Key key, System.Windows.Input.ModifierKeys modifierKeys);
	}
}
