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
using DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper;
using DeepLinkR.Ui.Models;
using MaterialDesignThemes.Wpf;
using NHotkey;

namespace DeepLinkR.Ui.ViewModels
{
	public class ShellViewModel : Screen, IHandle<ErrorEvent>
	{
		private IClipboardManager clipboardManager;
		private INHotkeyManagerMapper hotkeyManager;
		private ISnackbarMessageQueue sbMessageQueue;
		private bool isMenuBarVisible;
		private WindowState curWindowState;

		public ShellViewModel(IClipboardManager clipboardManager, INHotkeyManagerMapper hotkeyManager, ISnackbarMessageQueue snackbarMessageQueue)
		{
			this.clipboardManager = clipboardManager;
			this.hotkeyManager = hotkeyManager;
			this.SbMessageQueue = snackbarMessageQueue;

			// LoadConfiguration
			this.RegisterHotkey();

			// Load Menu items
			this.LoadMenuItems();
		}

		public ICommand MenuItemsSelectionChangedCommand => new SyncCommand(() => this.OnMenuItemsSelectionChanged());

		public ICommand ExitAppCommand => new SyncCommand(() => this.OnExitApp());

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

		public void Handle(ErrorEvent message)
		{
			this.sbMessageQueue.Enqueue(message.ErrorMessage);
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
					Content = new MainViewModel(),
					HorizontalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto,
					VerticalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto,
				},
				new NavigationMenuItem()
				{
					Name = "About",
					Content = new AboutViewModel(),
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
			// OpenMainWindow
		}

		private void OnMenuItemsSelectionChanged()
		{
			this.IsMenuBarVisible = false;
		}

		private void OnTitleBarDoubleClicked()
		{
			this.CurWindowState = this.CurWindowState != WindowState.Maximized ? WindowState.Maximized : WindowState.Normal;
		}

		private void OnExitApp()
		{
			System.Windows.Application.Current.Shutdown();
		}
	}
}
