﻿using System;
using System.Windows.Input;
using Caliburn.Micro;
using DeepLinkR.Ui.Events;
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
		public void ErrorEventsAreHandled()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var snackbarMessageQueue = Mock.Get((ISnackbarMessageQueue)mockObjects[nameof(ISnackbarMessageQueue)]);
			var vm = MockFactories.ShellViewModelFactory(mockObjects);

			vm.Handle(new ErrorEvent(new Exception(), "test"));

			snackbarMessageQueue.Verify(x => x.Enqueue(It.IsAny<object>()), Times.Once);
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
	}
}
