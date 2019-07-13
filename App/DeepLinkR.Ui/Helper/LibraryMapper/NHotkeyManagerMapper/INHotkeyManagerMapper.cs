using System;
using NHotkey.Wpf;

namespace DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper
{
	public interface INHotkeyManagerMapper
	{
		event EventHandler<HotkeyAlreadyRegisteredEventArgs> HotkeyAlreadyRegistered;

		void AddOrReplace(string name, System.Windows.Input.Key key, System.Windows.Input.ModifierKeys modifierKeys, EventHandler<NHotkey.HotkeyEventArgs> e);
	}
}
