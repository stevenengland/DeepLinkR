using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepLinkR.Ui.Events
{
	public class MappedHotKeyEventArgs : EventArgs
	{
		public MappedHotKeyEventArgs(string name)
		{
			this.Name = name;
		}

		public MappedHotKeyEventArgs(NHotkey.HotkeyEventArgs eventArgs)
		{
			this.Name = eventArgs.Name;
			this.Handled = eventArgs.Handled;
		}

		public string Name { get; }

		public bool Handled { get; set; }
	}
}
