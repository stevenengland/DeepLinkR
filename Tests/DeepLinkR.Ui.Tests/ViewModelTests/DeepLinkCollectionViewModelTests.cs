using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Caliburn.Micro;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Services.ClipboardManager;
using DeepLinkR.Core.Services.DeepLinkManager;
using DeepLinkR.Core.Tests.MockedObjects;
using DeepLinkR.Core.Types;
using DeepLinkR.Core.Types.Enums;
using DeepLinkR.Core.Types.EventArgs;
using DeepLinkR.Ui.Events;
using DeepLinkR.Ui.Tests.Mocks;
using DeepLinkR.Ui.ViewModels;
using Moq;
using Xunit;
using Action = Caliburn.Micro.Action;

namespace DeepLinkR.Ui.Tests.ViewModelTests
{
	public class DeepLinkCollectionViewModelTests
	{
		[Fact]
		public void VmIsSubscribedToEventAggregator()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);

			var vm = MockFactories.DeepLinkCollectionViewModelFactory(mockObjects);

			// Test to make sure subscribe was called on the event aggregator at least once
			eventAggregatorMock.Verify(x => x.Subscribe(vm), Times.Once);
		}

		[Fact]
		public void IncomingEventsAreHandled()
		{
			var mockObjects = MockFactories.GetMockObjects();

			var vm = MockFactories.DeepLinkCollectionViewModelFactory(mockObjects);

			vm.Handle(new HistoricalDeepLinkSelectedEvent(new List<DeepLinkMatch>()
			{
				MockedDeeplinkMatches.SimpleDeepLinkMatch,
			}));
		}

		[Fact]
		public void ClipboardUpdatesThatDoNotMatchAreProcessed()
		{
			var mockObjects = MockFactories.GetMockObjects();

			var clipboardManagerMock = Mock.Get((IClipboardManager)mockObjects[nameof(IClipboardManager)]);
			var deeplinkManagerMock = Mock.Get((IDeepLinkManager)mockObjects[nameof(IDeepLinkManager)]);
			var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);

			deeplinkManagerMock.Setup(x => x.GetDeepLinkMatches(It.IsAny<string>())).Returns((List<DeepLinkMatch>)null);
			eventAggregatorMock.Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<System.Action<System.Action>>())).Verifiable();

			var vm = MockFactories.DeepLinkCollectionViewModelFactory(mockObjects);

			clipboardManagerMock.Raise(x => x.ClipboardTextUpdateReceived += null, this, new ClipboardTextUpdateEventArgs("test"));

			deeplinkManagerMock.Verify(x => x.GetDeepLinkMatches(It.IsAny<string>()), Times.Exactly(1));
			eventAggregatorMock.Verify(x => x.Publish(It.IsAny<object>(), It.IsAny<System.Action<System.Action>>()), Times.Exactly(0));
		}

		[Fact]
		public void ClipboardUpdatesThatDoMatchAreProcessed()
		{
			var mockObjects = MockFactories.GetMockObjects();

			var clipboardManagerMock = Mock.Get((IClipboardManager)mockObjects[nameof(IClipboardManager)]);
			var deeplinkManagerMock = Mock.Get((IDeepLinkManager)mockObjects[nameof(IDeepLinkManager)]);
			var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);

			deeplinkManagerMock.Setup(x => x.GetDeepLinkMatches(It.IsAny<string>())).Returns(new List<DeepLinkMatch>() { MockedDeeplinkMatches.SimpleDeepLinkMatch });
			eventAggregatorMock.Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<System.Action<System.Action>>())).Verifiable();

			var vm = MockFactories.DeepLinkCollectionViewModelFactory(mockObjects);

			clipboardManagerMock.Raise(x => x.ClipboardTextUpdateReceived += null, this, new ClipboardTextUpdateEventArgs("test"));

			deeplinkManagerMock.Verify(x => x.GetDeepLinkMatches(It.IsAny<string>()), Times.Exactly(1));
			eventAggregatorMock.Verify(x => x.Publish(It.IsAny<object>(), It.IsAny<System.Action<System.Action>>()), Times.Exactly(1));
		}

		[Fact]
		public void SortedListOfMatchesByCategoryIsCreated()
		{
			var mockObjects = MockFactories.GetMockObjects();

			var vm = MockFactories.DeepLinkCollectionViewModelFactory(mockObjects);

			vm.Sideload(MockedDeeplinkMatches.OrderTestList, DeepLinkSortOrder.Category);

			var firstLevelOneItem = vm.HierarchicalLinks.First();
			var lastLevelOneItem = vm.HierarchicalLinks.Last();

			Assert.Equal("Cat1", firstLevelOneItem.Name);
			Assert.Equal("Key1", firstLevelOneItem.SecondLinkHierarchies.First().Name);
			Assert.Equal("Key3", firstLevelOneItem.SecondLinkHierarchies.Last().Name);
			Assert.Equal("Cat2", lastLevelOneItem.Name);
			Assert.Equal("Key1", lastLevelOneItem.SecondLinkHierarchies.First().Name);
			Assert.Equal("Key3", lastLevelOneItem.SecondLinkHierarchies.Last().Name);

			vm.Sideload(MockedDeeplinkMatches.OrderTestList, DeepLinkSortOrder.Category, true);

			firstLevelOneItem = vm.HierarchicalLinks.First();
			lastLevelOneItem = vm.HierarchicalLinks.Last();

			Assert.Equal("Cat2", firstLevelOneItem.Name);
			Assert.Equal("Key3", firstLevelOneItem.SecondLinkHierarchies.First().Name);
			Assert.Equal("Key1", firstLevelOneItem.SecondLinkHierarchies.Last().Name);
			Assert.Equal("Cat1", lastLevelOneItem.Name);
			Assert.Equal("Key3", lastLevelOneItem.SecondLinkHierarchies.First().Name);
			Assert.Equal("Key1", lastLevelOneItem.SecondLinkHierarchies.Last().Name);
		}

		[Fact]
		public void ChangingSortOrderIsHandled()
		{
			var mockObjects = MockFactories.GetMockObjects();

			var vm = MockFactories.DeepLinkCollectionViewModelFactory(mockObjects);

			vm.Sideload(MockedDeeplinkMatches.OrderTestList, DeepLinkSortOrder.Category);

			var firstLevelOneItem = vm.HierarchicalLinks.First();

			Assert.Equal("Cat1", firstLevelOneItem.Name);

			vm.ChangeSortOrderDirectionCommand.Execute(null);

			firstLevelOneItem = vm.HierarchicalLinks.First();

			Assert.Equal("Cat2", firstLevelOneItem.Name);
		}

		[Fact]
		public void SortedListOfMatchesByKeyIsCreated()
		{
			var mockObjects = MockFactories.GetMockObjects();

			var vm = MockFactories.DeepLinkCollectionViewModelFactory(mockObjects);

			vm.Sideload(MockedDeeplinkMatches.OrderTestList, DeepLinkSortOrder.Key);

			var firstLevelOneItem = vm.HierarchicalLinks.First();
			var lastLevelOneItem = vm.HierarchicalLinks.Last();

			Assert.Equal("Key1", firstLevelOneItem.Name);
			Assert.Equal("Cat1", firstLevelOneItem.SecondLinkHierarchies.First().Name);
			Assert.Equal("Cat2", firstLevelOneItem.SecondLinkHierarchies.Last().Name);
			Assert.Equal("Key3", lastLevelOneItem.Name);
			Assert.Equal("Cat1", lastLevelOneItem.SecondLinkHierarchies.First().Name);
			Assert.Equal("Cat2", lastLevelOneItem.SecondLinkHierarchies.Last().Name);

			vm.Sideload(MockedDeeplinkMatches.OrderTestList, DeepLinkSortOrder.Key, true);

			firstLevelOneItem = vm.HierarchicalLinks.First();
			lastLevelOneItem = vm.HierarchicalLinks.Last();

			Assert.Equal("Key3", firstLevelOneItem.Name);
			Assert.Equal("Cat2", firstLevelOneItem.SecondLinkHierarchies.First().Name);
			Assert.Equal("Cat1", firstLevelOneItem.SecondLinkHierarchies.Last().Name);
			Assert.Equal("Key1", lastLevelOneItem.Name);
			Assert.Equal("Cat2", lastLevelOneItem.SecondLinkHierarchies.First().Name);
			Assert.Equal("Cat1", lastLevelOneItem.SecondLinkHierarchies.Last().Name);
		}

		[Fact]
		public void ChangingDeepLinkSortOrderIsHandled()
		{
			var mockObjects = MockFactories.GetMockObjects();

			var vm = MockFactories.DeepLinkCollectionViewModelFactory(mockObjects);

			vm.Sideload(MockedDeeplinkMatches.OrderTestList);

			var firstLevelOneItem = vm.HierarchicalLinks.First();

			Assert.Equal("Cat1", firstLevelOneItem.Name);

			vm.ChangeDeepLinkSortOrderDirectionCommand.Execute(null);

			firstLevelOneItem = vm.HierarchicalLinks.First();

			Assert.Equal("Key1", firstLevelOneItem.Name);
		}

		[Fact]
		public void CopyingToClipboardIsHandled()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var clipboardManagerMock = Mock.Get((IClipboardManager)mockObjects[nameof(IClipboardManager)]);
			clipboardManagerMock.Setup(x => x.CopyTextToClipboard(It.IsAny<string>())).Verifiable();

			var vm = MockFactories.DeepLinkCollectionViewModelFactory(mockObjects);

			vm.CopyLinkToClipboardCommand.Execute("test");

			clipboardManagerMock.Verify(x => x.CopyTextToClipboard(It.IsAny<string>()), Times.Once);
		}
	}
}
