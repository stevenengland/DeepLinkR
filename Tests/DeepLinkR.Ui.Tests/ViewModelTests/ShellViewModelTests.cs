using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Caliburn.Micro;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Services.ClipboardManager;
using DeepLinkR.Core.Services.DeepLinkManager;
using DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper;
using DeepLinkR.Ui.Tests.Mocks;
using DeepLinkR.Ui.ViewModels;
using MaterialDesignThemes.Wpf;
using Moq;
using Xunit;

namespace DeepLinkR.Ui.Tests.ViewModelTests
{
	public class ShellViewModelTests
	{
		[Fact]
		public void MenuItemChangesAreHandled()
		{
			var mockObjects = this.GetMockObjects();

			var vm = this.ViewModelFactory(mockObjects);

			vm.MenuItemsSelectionChangedCommand.Execute(null);

			Assert.False(vm.IsMenuBarVisible);
		}

		private Dictionary<string, object> GetMockObjects()
		{
			return new Dictionary<string, object>()
			{
				{ nameof(IClipboardManager), MockFactories.GetClipboardManager() },
				{ nameof(INHotkeyManagerMapper), MockFactories.GetINHotkeyManagerMapper() },
				{ nameof(ISnackbarMessageQueue), MockFactories.GetISnackbarMessageQueue() },
			};
		}

		private ShellViewModel ViewModelFactory(Dictionary<string, object> mockObjects)
		{
			return new ShellViewModel(
				(IClipboardManager)mockObjects[nameof(IClipboardManager)],
				(INHotkeyManagerMapper)mockObjects[nameof(INHotkeyManagerMapper)],
				(ISnackbarMessageQueue)mockObjects[nameof(ISnackbarMessageQueue)]
				);
		}
	}
}
