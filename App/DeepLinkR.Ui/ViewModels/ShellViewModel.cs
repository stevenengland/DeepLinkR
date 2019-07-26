using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using DeepLinkR.Core.Helper.SyncCommand;
using DeepLinkR.Core.Services.ClipboardManager;
using DeepLinkR.Core.Services.LoggerManager;
using DeepLinkR.Ui.Events;
using DeepLinkR.Ui.Helper.LibraryMapper.DialogHostMapper;
using DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper;
using DeepLinkR.Ui.Models;
using DeepLinkR.Ui.Views;
using MaterialDesignThemes.Wpf;
using NHotkey;
using NHotkey.Wpf;

namespace DeepLinkR.Ui.ViewModels
{
	public class ShellViewModel : Screen, IHandleWithTask<ErrorEvent>
	{
		private IClipboardManager clipboardManager;
		private INHotkeyManagerMapper hotkeyManager;
		private ISnackbarMessageQueue sbMessageQueue;
		private IEventAggregator eventAggregator;
		private IDialogHostMapper dialogHostMapper;
		private ILoggerManager loggerManager;
		private bool isMenuBarVisible;
		private WindowState curWindowState;
		private MainViewModel mainViewModel;
		private AboutViewModel aboutViewModel;
		private ErrorEvent lastErrorEvent;

		public ShellViewModel(
			IClipboardManager clipboardManager,
			INHotkeyManagerMapper hotkeyManager,
			ISnackbarMessageQueue snackbarMessageQueue,
			IEventAggregator eventAggregator,
			IDialogHostMapper dialogHostMapper,
			ILoggerManager loggerManager,
			MainViewModel mainViewModel,
			AboutViewModel aboutViewModel)
		{
			this.clipboardManager = clipboardManager;
			this.hotkeyManager = hotkeyManager;
			this.SbMessageQueue = snackbarMessageQueue;
			this.eventAggregator = eventAggregator;
			this.dialogHostMapper = dialogHostMapper;
			this.loggerManager = loggerManager;
			this.mainViewModel = mainViewModel;
			this.aboutViewModel = aboutViewModel;

			this.eventAggregator.Subscribe(this);

			// Register hotkey
			this.RegisterHotkey();

			// Load Menu items
			this.LoadMenuItems();
		}

		public ICommand MenuItemsSelectionChangedCommand => new SyncCommand(() => this.OnMenuItemsSelectionChanged());

		public ICommand ExitAppCommand => new SyncCommand(() => this.OnExitApp());

		public ICommand MinimizeAppCommand => new SyncCommand(() => this.OnMinimizeApp());

		public ICommand MaximizeAppCommand => new SyncCommand(() => this.OnMaximizeApp());

		public ICommand MoveToTrayCommand => new SyncCommand(() => this.OnMoveToTray());

		public ICommand TitleBarDoubleClickedCommand => new SyncCommand(() => this.OnTitleBarDoubleClicked());

		public ISnackbarMessageQueue SbMessageQueue { get => this.sbMessageQueue; private set => this.sbMessageQueue = value; }

		public NavigationMenuItem[] MenuItems { get; private set; }

		public bool IsMenuBarVisible
		{
			get => this.isMenuBarVisible;
			set
			{
				this.isMenuBarVisible = value;
				this.NotifyOfPropertyChange(() => this.IsMenuBarVisible);
			}
		}

		public WindowState CurWindowState
		{
			get => this.curWindowState;
			set
			{
				this.curWindowState = value;
				this.NotifyOfPropertyChange(() => this.CurWindowState);
			}
		}

		public async Task Handle(ErrorEvent errorEvent)
		{
			// ToDo: Log the Exception
			this.lastErrorEvent = errorEvent;
			this.loggerManager.Error(errorEvent.Exception);
			if (errorEvent.ApplicationMustShutdown)
			{
				var result = await this.dialogHostMapper.Show(this.dialogHostMapper.GetErrorView(errorEvent.ErrorMessage + "\n\n\nApplication needs to shutdown itself"), "RootDialog");
				this.GracefulShutdown();
			}
			else
			{
				this.SbMessageQueue.Enqueue<ErrorEvent>("An error occured!", "Details", async (arg) => await this.ShowError(arg), errorEvent);
			}
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);

			// All stuff that uses error handling should come here to make use of the DialogHost, that is only available after the view loaded.
			if (this.lastErrorEvent != null)
			{
				this.SbMessageQueue.Enqueue("Application loaded with errors.");
				if (this.lastErrorEvent.ApplicationMustShutdown)
				{
					var result = await this.dialogHostMapper.Show(this.dialogHostMapper.GetErrorView(this.lastErrorEvent.ErrorMessage + "\n\n\nApplication needs to shutdown itself"), "RootDialog");
					this.GracefulShutdown();
				}
			}
		}

		private async Task ShowError(ErrorEvent errorEvent)
		{
			var result = await this.dialogHostMapper.Show(this.dialogHostMapper.GetErrorView(errorEvent.ErrorMessage), "RootDialog");
		}

		private void LoadMenuItems()
		{
			this.MenuItems = new[]
			{
				new NavigationMenuItem()
				{
					Name = "Deeplinks",
					Content = this.mainViewModel,
					HorizontalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto,
					VerticalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto,
				},
				new NavigationMenuItem()
				{
					Name = "About",
					Content = this.aboutViewModel,
					HorizontalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto,
					VerticalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto,
				},
			};
		}

		private void RegisterHotkey()
		{
			this.hotkeyManager.HotkeyAlreadyRegistered += this.OnHotkeyAlreadyRegistered;
			this.hotkeyManager.AddOrReplace("OpenDeepLinkR", Key.Space, ModifierKeys.Alt);
			this.hotkeyManager.HotKeyPressed += this.OnDeepLinkROpen;
		}

		private void OnHotkeyAlreadyRegistered(object sender, MappedHotkeyAlreadyRegisteredEventArgs e)
		{
			this.eventAggregator.PublishOnUIThread(new ErrorEvent(null, $"The hotkey is already registered by another application ({e.Name})."));
		}

		private void OnDeepLinkROpen(object sender, MappedHotKeyEventArgs e)
		{
			if (this.CurWindowState == WindowState.Minimized)
			{
				this.CurWindowState = WindowState.Normal;
			}

			e.Handled = true;
		}

		private void OnMenuItemsSelectionChanged()
		{
			this.IsMenuBarVisible = false;
		}

		private void OnTitleBarDoubleClicked()
		{
			this.CurWindowState = this.CurWindowState != WindowState.Maximized ? WindowState.Maximized : WindowState.Normal;
		}

		private void OnMinimizeApp()
		{
			this.CurWindowState = WindowState.Minimized;
		}

		private void OnMaximizeApp()
		{
			this.CurWindowState = WindowState.Maximized;
		}

		private void OnMoveToTray()
		{
			Application.Current.MainWindow.ShowInTaskbar = false;
			Application.Current.MainWindow.Visibility = Visibility.Hidden;
		}

		private void OnExitApp()
		{
			this.GracefulShutdown();
		}

		private void GracefulShutdown()
		{
			System.Windows.Application.Current.Shutdown();
		}
	}
}
