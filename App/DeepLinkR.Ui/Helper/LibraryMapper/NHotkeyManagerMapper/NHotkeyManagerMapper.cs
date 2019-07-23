using System;
using DeepLinkR.Ui.Events;
using NHotkey;
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

		public event EventHandler<HotKeyEventArgs> HotKeyPressed;

		public void AddOrReplace(string name, System.Windows.Input.Key key, System.Windows.Input.ModifierKeys modifierKeys)
		{
			HotkeyManager.Current.AddOrReplace(name, key, modifierKeys, this.OnHotKeyPressed);
		}

		private void OnHotKeyPressed(object sender, HotkeyEventArgs e)
		{
			this.HotKeyPressed?.Invoke(this, new HotKeyEventArgs(e));
		}
	}
}
