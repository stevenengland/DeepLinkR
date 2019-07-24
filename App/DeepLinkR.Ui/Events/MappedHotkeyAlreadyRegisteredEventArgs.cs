using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepLinkR.Ui.Events
{
	public class MappedHotkeyAlreadyRegisteredEventArgs : EventArgs
	{
		public MappedHotkeyAlreadyRegisteredEventArgs(string name)
		{
			this.Name = name;
		}

		public string Name { get; set; }
	}
}
