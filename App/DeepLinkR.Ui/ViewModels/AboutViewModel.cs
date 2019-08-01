using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace DeepLinkR.Ui.ViewModels
{
	public class AboutViewModel : Screen
	{
		private string appVersion;

		public AboutViewModel()
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			Version version = assembly.GetName().Version;
			this.AppVersion = version.Major + "." + version.Minor + "." + version.Build;
		}

		public string AppVersion
		{
			get => this.appVersion;
			set
			{
				if (value == this.appVersion)
				{
					return;
				}

				this.appVersion = value;
				this.NotifyOfPropertyChange(() => this.AppVersion);
			}
		}
	}
}
