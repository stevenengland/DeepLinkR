using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using DeepLinkR.Core.Services.ClipboardManager;
using DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper;
using DeepLinkR.Ui.Models;
using MaterialDesignThemes.Wpf;
using NHotkey;

namespace DeepLinkR.Ui.ViewModels
{
	public class ShellViewModel : Screen
	{
		private IClipboardManager clipboardManager;
		private INHotkeyManagerMapper hotkeyManager;
		private ISnackbarMessageQueue sbMessageQueue;
		private bool isMenuBarVisible;

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

		public void OnMenuItemsSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.IsMenuBarVisible = false;
		}

		protected override void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			this.SbMessageQueue.Enqueue("Application successful loaded");
		}

		private void LoadMenuItems()
		{
			this.MenuItems = new[]
			{
				new NavigationMenuItem()
				{
					Name = "Main",
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
	}
}
