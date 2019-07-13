using System;
using NHotkey.Wpf;

namespace DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper
{
	public class NHotkeyManagerMapper : INHotkeyManagerMapper
	{
		public NHotkeyManagerMapper()
		{
			HotkeyManager.HotkeyAlreadyRegistered += this.HotkeyAlreadyRegistered;
		}

		public event EventHandler<HotkeyAlreadyRegisteredEventArgs> HotkeyAlreadyRegistered;

		public void AddOrReplace(string name, System.Windows.Input.Key key, System.Windows.Input.ModifierKeys modifierKeys, EventHandler<NHotkey.HotkeyEventArgs> e)
		{
			HotkeyManager.Current.AddOrReplace(name, key, modifierKeys, e);
		}
	}
}
