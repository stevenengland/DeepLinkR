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
			HotkeyManager.HotkeyAlreadyRegistered += this.OnHotkeyAlreadyRegistered;
		}

		public event EventHandler<MappedHotkeyAlreadyRegisteredEventArgs> HotkeyAlreadyRegistered;

		public event EventHandler<MappedHotKeyEventArgs> HotKeyPressed;

		public void AddOrReplace(string name, System.Windows.Input.Key key, System.Windows.Input.ModifierKeys modifierKeys)
		{
			try
			{
				HotkeyManager.Current.AddOrReplace(name, key, modifierKeys, this.OnHotKeyPressed);
			}
			catch (HotkeyAlreadyRegisteredException e)
			{
				this.HotkeyAlreadyRegistered?.Invoke(this, new MappedHotkeyAlreadyRegisteredEventArgs(e.Name));
			}
		}

		private void OnHotKeyPressed(object sender, HotkeyEventArgs e)
		{
			this.HotKeyPressed?.Invoke(this, new MappedHotKeyEventArgs(e));
		}

		private void OnHotkeyAlreadyRegistered(object sender, HotkeyAlreadyRegisteredEventArgs e)
		{
			this.HotkeyAlreadyRegistered?.Invoke(this, new MappedHotkeyAlreadyRegisteredEventArgs(e.Name));
		}
	}
}
