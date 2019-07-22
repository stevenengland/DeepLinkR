using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using DeepLinkR.Ui.Events;
using DeepLinkR.Ui.Helper.LibraryMapper.DialogHostMapper;
using DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper;
using DeepLinkR.Ui.Tests.Mocks;
using MaterialDesignThemes.Wpf;
using Moq;
using NHotkey;
using Xunit;

namespace DeepLinkR.Ui.Tests.ViewModelTests
{
	public class ShellViewModelTests
	{
		[Fact]
		public void VmIsSubscribedToEventAggregator()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);

			var vm = MockFactories.ShellViewModelFactory(mockObjects);

			// Test to make sure subscribe was called on the event aggregator at least once
			eventAggregatorMock.Verify(x => x.Subscribe(vm), Times.Once);
		}

		[Fact]
		public void HotKeyIsRegistered()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var hotkeyManager = Mock.Get((INHotkeyManagerMapper)mockObjects[nameof(INHotkeyManagerMapper)]);
			hotkeyManager.Setup(x =>
				x.AddOrReplace(It.IsAny<string>(), It.IsAny<Key>(), It.IsAny<ModifierKeys>(), It.IsAny<EventHandler<HotkeyEventArgs>>()));

			var vm = MockFactories.ShellViewModelFactory(mockObjects);

			// Test to make sure subscribe was called on the event aggregator at least once
			hotkeyManager.Verify(x => x.AddOrReplace(It.IsAny<string>(), It.IsAny<Key>(), It.IsAny<ModifierKeys>(), It.IsAny<EventHandler<HotkeyEventArgs>>()), Times.Once);
		}

		[Fact]
		public async Task ErrorEventsAreHandled()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var dialogHostMapper = Mock.Get((IDialogHostMapper)mockObjects[nameof(IDialogHostMapper)]);
			var vm = MockFactories.ShellViewModelFactory(mockObjects);

			await vm.Handle(new ErrorEvent(new Exception(), "test"));

			dialogHostMapper.Verify(x => x.Show(It.IsAny<object>(), It.IsAny<object>()), Times.Once);
		}

		[Fact]
		public void MenuItemChangesAreHandled()
		{
			var mockObjects = MockFactories.GetMockObjects();

			var vm = MockFactories.ShellViewModelFactory(mockObjects);

			vm.MenuItemsSelectionChangedCommand.Execute(null);

			Assert.False(vm.IsMenuBarVisible);
		}

		[Fact]
		public void TitleBarDoubleClicksChangesWindowState()
		{
			var mockObjects = MockFactories.GetMockObjects();

			var vm = MockFactories.ShellViewModelFactory(mockObjects);

			var curState = vm.CurWindowState;

			vm.TitleBarDoubleClickedCommand.Execute(null);

			Assert.NotEqual(curState, vm.CurWindowState);
		}

		[Fact]
		public void WindowGetsMaximized()
		{
			var mockObjects = MockFactories.GetMockObjects();

			var vm = MockFactories.ShellViewModelFactory(mockObjects);

			vm.MaximizeAppCommand.Execute(null);

			Assert.Equal(WindowState.Maximized, vm.CurWindowState);
		}

		[Fact]
		public void WindowGetsMinimized()
		{
			var mockObjects = MockFactories.GetMockObjects();

			var vm = MockFactories.ShellViewModelFactory(mockObjects);

			vm.MinimizeAppCommand.Execute(null);

			Assert.Equal(WindowState.Minimized, vm.CurWindowState);
		}
	}
}
