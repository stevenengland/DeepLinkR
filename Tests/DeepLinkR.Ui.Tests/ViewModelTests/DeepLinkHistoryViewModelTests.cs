using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using DeepLinkR.Core.Tests.MockedObjects;
using DeepLinkR.Core.Types;
using DeepLinkR.Ui.Events;
using DeepLinkR.Ui.Models;
using DeepLinkR.Ui.Tests.Mocks;
using Moq;
using Xunit;

namespace DeepLinkR.Ui.Tests.ViewModelTests
{
	public class DeepLinkHistoryViewModelTests
	{
		[Fact]
		public void VmIsSubscribedToEventAggregator()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);

			var vm = MockFactories.DeepLinkHistoryViewModelFactory(mockObjects);

			// Test to make sure subscribe was called on the event aggregator at least once
			eventAggregatorMock.Verify(x => x.Subscribe(vm), Times.Once);
		}

		[Fact]
		public void SelectedEntryIsPublishedAfterSelectionChanged()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);

			eventAggregatorMock.Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<System.Action<System.Action>>())).Verifiable();

			var vm = MockFactories.DeepLinkHistoryViewModelFactory(mockObjects);

			vm.HistoryItemsSelectionChangedCommand.Execute(new HistoryEntry()
			{
				DeepLinkMatches = new List<DeepLinkMatch>()
				{
					MockedDeeplinkMatches.SimpleDeepLinkMatch,
				},
				DeepLinkMatchValue = "test",
			});

			eventAggregatorMock.Verify(x => x.Publish(It.IsAny<object>(), It.IsAny<System.Action<System.Action>>()), Times.Exactly(1));
			Assert.Equal("test", vm.HistoryEntry.DeepLinkMatchValue);
		}

		[Fact]
		public void IncomingEventsAreHandled()
		{
			var mockObjects = MockFactories.GetMockObjects();

			var vm = MockFactories.DeepLinkHistoryViewModelFactory(mockObjects);

			vm.Handle(new DeepLinkMatchesUpdatedEvent(
				new List<DeepLinkMatch>()
				{
					MockedDeeplinkMatches.SimpleDeepLinkMatch,
				},
				"test"));

			Assert.NotEmpty(vm.HistoryEntryList);
		}

		[Fact]
		public void KnownEntriesDoNotUpdateList()
		{
			var mockObjects = MockFactories.GetMockObjects();

			var vm = MockFactories.DeepLinkHistoryViewModelFactory(mockObjects);
			vm.HistoryEntryList.Add(new HistoryEntry()
			{
				DeepLinkMatches = new List<DeepLinkMatch>()
				{
					MockedDeeplinkMatches.SimpleDeepLinkMatch,
				},
				DeepLinkMatchValue = "test",
			});

			vm.Handle(new DeepLinkMatchesUpdatedEvent(
				new List<DeepLinkMatch>()
				{
					MockedDeeplinkMatches.SimpleDeepLinkMatch,
				},
				"test"));

			Assert.Single(vm.HistoryEntryList);
		}

		[Fact]
		public void ListThresholdIsHandled()
		{
			var mockObjects = MockFactories.GetMockObjects();

			var vm = MockFactories.DeepLinkHistoryViewModelFactory(mockObjects);
			for (int i = 1; i <= 10; i++)
			{
				vm.Handle(new DeepLinkMatchesUpdatedEvent(
					new List<DeepLinkMatch>()
					{
						new DeepLinkMatch(),
					},
					"test" + i));
			}

			Assert.Equal(10, vm.HistoryEntryList.Count);

			vm.Handle(new DeepLinkMatchesUpdatedEvent(
				new List<DeepLinkMatch>()
				{
					MockedDeeplinkMatches.SimpleDeepLinkMatch,
				},
				"test100"));

			Assert.Equal(10, vm.HistoryEntryList.Count);
			Assert.Equal("test2", vm.HistoryEntryList.Last().DeepLinkMatchValue);
			Assert.Equal("test100", vm.HistoryEntryList.First().DeepLinkMatchValue);
		}
	}
}
