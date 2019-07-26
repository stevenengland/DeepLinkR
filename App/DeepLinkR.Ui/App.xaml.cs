using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace DeepLinkR.Ui
{
	/// <summary>
	/// Interaction logic for App.xaml.
	/// </summary>
	public partial class App : Application
	{
		private TaskbarIcon notifyIcon;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			// create the notifyicon (it's a resource declared in NotifyIconResources.xaml
			this.notifyIcon = (TaskbarIcon)this.FindResource("NotifyIcon");
		}

		protected override void OnExit(ExitEventArgs e)
		{
			// the icon would clean up automatically, but this is cleaner
			this.notifyIcon.Dispose();
			base.OnExit(e);
		}
	}
}
