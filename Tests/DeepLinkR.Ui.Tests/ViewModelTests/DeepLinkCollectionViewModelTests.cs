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
			var mockObjects = this.GetMockObjects();
			var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);

			var vm = this.ViewModelFactory(mockObjects);

			// Test to make sure subscribe was called on the event aggregator at least once
			eventAggregatorMock.Verify(x => x.Subscribe(vm), Times.Once);
		}

		[Fact]
		public void IncomingEventsAreHandled()
		{
			var mockObjects = this.GetMockObjects();

			var vm = this.ViewModelFactory(mockObjects);

			vm.Handle(new HistoricalDeepLinkSelectedEvent(new List<DeepLinkMatch>()
			{
				MockedDeeplinkMatches.SimpleDeepLinkMatch,
			}));
		}

		[Fact]
		public void ClipboardUpdatesThatDoNotMatchAreProcessed()
		{
			var mockObjects = this.GetMockObjects();

			var clipboardManagerMock = Mock.Get((IClipboardManager)mockObjects[nameof(IClipboardManager)]);
			var deeplinkManagerMock = Mock.Get((IDeepLinkManager)mockObjects[nameof(IDeepLinkManager)]);
			var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);

			deeplinkManagerMock.Setup(x => x.GetDeepLinkMatches(It.IsAny<string>())).Returns((List<DeepLinkMatch>)null);
			eventAggregatorMock.Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<System.Action<System.Action>>())).Verifiable();

			var vm = this.ViewModelFactory(mockObjects);

			clipboardManagerMock.Raise(x => x.ClipboardTextUpdateReceived += null, this, new ClipboardTextUpdateEventArgs("test"));

			deeplinkManagerMock.Verify(x => x.GetDeepLinkMatches(It.IsAny<string>()), Times.Exactly(1));
			eventAggregatorMock.Verify(x => x.Publish(It.IsAny<object>(), It.IsAny<System.Action<System.Action>>()), Times.Exactly(0));
		}

		[Fact]
		public void ClipboardUpdatesThatDoMatchAreProcessed()
		{
			var mockObjects = this.GetMockObjects();

			var clipboardManagerMock = Mock.Get((IClipboardManager)mockObjects[nameof(IClipboardManager)]);
			var deeplinkManagerMock = Mock.Get((IDeepLinkManager)mockObjects[nameof(IDeepLinkManager)]);
			var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);

			deeplinkManagerMock.Setup(x => x.GetDeepLinkMatches(It.IsAny<string>())).Returns(new List<DeepLinkMatch>() { MockedDeeplinkMatches.SimpleDeepLinkMatch });
			eventAggregatorMock.Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<System.Action<System.Action>>())).Verifiable();

			var vm = this.ViewModelFactory(mockObjects);

			clipboardManagerMock.Raise(x => x.ClipboardTextUpdateReceived += null, this, new ClipboardTextUpdateEventArgs("test"));

			deeplinkManagerMock.Verify(x => x.GetDeepLinkMatches(It.IsAny<string>()), Times.Exactly(1));
			eventAggregatorMock.Verify(x => x.Publish(It.IsAny<object>(), It.IsAny<System.Action<System.Action>>()), Times.Exactly(1));
		}

		[Fact]
		public void SortedListOfMatchesIsCreated()
		{
			var mockObjects = this.GetMockObjects();

			var vm = this.ViewModelFactory(mockObjects);

			vm.Sideload(MockedDeeplinkMatches.OrderTestList);

			var firstLevelOneItem = vm.HierarchicalLinks.First();
			var lastLevelOneItem = vm.HierarchicalLinks.Last();

			Assert.Equal("Cat1", firstLevelOneItem.Name);
			Assert.Equal("Key1", firstLevelOneItem.SecondLinkHierarchies.First().Name);
			Assert.Equal("Key3", firstLevelOneItem.SecondLinkHierarchies.Last().Name);
			Assert.Equal("Cat2", lastLevelOneItem.Name);
			Assert.Equal("Key1", lastLevelOneItem.SecondLinkHierarchies.First().Name);
			Assert.Equal("Key3", lastLevelOneItem.SecondLinkHierarchies.Last().Name);
		}

		private Dictionary<string, object> GetMockObjects()
		{
			return new Dictionary<string, object>()
			{
				{ nameof(IConfigurationCollection), MockFactories.GetConfigurationCollection() },
				{ nameof(IClipboardManager), MockFactories.GetClipboardManager() },
				{ nameof(IDeepLinkManager), MockFactories.GetDeepLinkManager() },
				{ nameof(IMapper), MockFactories.GetMapper() },
				{ nameof(IEventAggregator), MockFactories.GetEventAggregator() },
			};
		}

		private DeepLinkCollectionViewModel ViewModelFactory(Dictionary<string, object> mockObjects)
		{
			return new DeepLinkCollectionViewModel(
				(IConfigurationCollection)mockObjects[nameof(IConfigurationCollection)],
				(IClipboardManager)mockObjects[nameof(IClipboardManager)],
				(IDeepLinkManager)mockObjects[nameof(IDeepLinkManager)],
				(IMapper)mockObjects[nameof(IMapper)],
				(IEventAggregator)mockObjects[nameof(IEventAggregator)]);
		}
	}
}
