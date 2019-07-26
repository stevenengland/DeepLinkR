using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using DeepLinkR.Core.Helper.SyncCommand;
using DeepLinkR.Ui.Helper.Extensions;
using DeepLinkR.Ui.Views;

namespace DeepLinkR.Ui.ViewModels
{
	/// <summary>
	/// Provides bindable properties and commands for the NotifyIcon. In this sample, the
	/// view model is assigned to the NotifyIcon in XAML. Alternatively, the startup routing
	/// in App.xaml.cs could have created this view model, and assigned it to the NotifyIcon.
	/// </summary>
	public class NotifyIconViewModel 
	{
		/// <summary>
		/// Shows a window, if none is already open.
		/// </summary>
		public ICommand ShowWindowCommand => new SyncCommand(
			() =>
			{
				Application.Current.MainWindow.ShowInTaskbar = true;
				Application.Current.MainWindow.Visibility = Visibility.Visible;
			},
			() => Application.Current.MainWindow.Visibility != Visibility.Visible);

		/// <summary>
		/// Hides the main window. This command is only enabled if a window is open.
		/// </summary>
		public ICommand HideWindowCommand => new SyncCommand(
			() =>
			{
				Application.Current.MainWindow.ShowInTaskbar = false;
				Application.Current.MainWindow.Visibility = Visibility.Hidden;
			},
			() => Application.Current.MainWindow.Visibility == Visibility.Visible);

		/// <summary>
		/// Shuts down the application.
		/// </summary>
		public ICommand ExitApplicationCommand
		{
			get
			{
				return new SyncCommand(
					() =>
					{
						Application.Current.Shutdown();
					});
			}
		}
	}
}
