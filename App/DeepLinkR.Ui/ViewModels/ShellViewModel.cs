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
using DeepLinkR.Ui.Events;
using DeepLinkR.Ui.Helper.LibraryMapper.DialogHostMapper;
using DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper;
using DeepLinkR.Ui.Models;
using DeepLinkR.Ui.Views;
using MaterialDesignThemes.Wpf;
using NHotkey;

namespace DeepLinkR.Ui.ViewModels
{
	public class ShellViewModel : Screen, IHandleWithTask<ErrorEvent>
	{
		private IClipboardManager clipboardManager;
		private INHotkeyManagerMapper hotkeyManager;
		private ISnackbarMessageQueue sbMessageQueue;
		private IEventAggregator eventAggregator;
		private IDialogHostMapper dialogHostMapper;
		private bool isMenuBarVisible;
		private WindowState curWindowState;
		private MainViewModel mainViewModel;
		private AboutViewModel aboutViewModel;

		public ShellViewModel(
			IClipboardManager clipboardManager,
			INHotkeyManagerMapper hotkeyManager,
			ISnackbarMessageQueue snackbarMessageQueue,
			IEventAggregator eventAggregator,
			IDialogHostMapper dialogHostMapper,
			MainViewModel mainViewModel,
			AboutViewModel aboutViewModel)
		{
			this.clipboardManager = clipboardManager;
			this.hotkeyManager = hotkeyManager;
			this.SbMessageQueue = snackbarMessageQueue;
			this.eventAggregator = eventAggregator;
			this.dialogHostMapper = dialogHostMapper;
			this.mainViewModel = mainViewModel;
			this.aboutViewModel = aboutViewModel;

			this.eventAggregator.Subscribe(this);

			// LoadConfiguration
			this.RegisterHotkey();

			// Load Menu items
			this.LoadMenuItems();
		}

		public ICommand MenuItemsSelectionChangedCommand => new SyncCommand(() => this.OnMenuItemsSelectionChanged());

		public ICommand ExitAppCommand => new SyncCommand(() => this.OnExitApp());

		public ICommand MinimizeAppCommand => new SyncCommand(() => this.OnMinimizeApp());

		public ICommand MaximizeAppCommand => new SyncCommand(() => this.OnMaximizeApp());

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

		public async Task Handle(ErrorEvent message)
		{
			// var view = new ErrorView()
			// {
			// DataContext = new ErrorViewModel(message.ErrorMessage),
			// };
			// var result = await this.dialogHostMapper.Show(view, "RootDialog");

			// ToDo: Log the Exception
			var result = await this.dialogHostMapper.Show(this.dialogHostMapper.GetErrorView(message.ErrorMessage), "RootDialog");
		}

		protected override void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			this.SbMessageQueue.Enqueue("Application successfully loaded");
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
			this.hotkeyManager.AddOrReplace("OpenDeepLinkR", Key.Space, ModifierKeys.Alt, this.OnDeepLinkROpen);
		}

		private void OnDeepLinkROpen(object sender, HotkeyEventArgs e)
		{
			if (this.CurWindowState == WindowState.Minimized)
			{
				this.CurWindowState = WindowState.Normal;
			}
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

		private void OnExitApp()
		{
			System.Windows.Application.Current.Shutdown();
		}
	}
}
