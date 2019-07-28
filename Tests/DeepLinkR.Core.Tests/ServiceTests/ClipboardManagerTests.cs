using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Helper.LibraryMapper.SharpClipboardMapper;
using DeepLinkR.Core.Helper.LibraryMapper.TextCopyMapper;
using DeepLinkR.Core.Tests.MockedObjects;
using DeepLinkR.Core.Tests.Mocks;
using DeepLinkR.Core.Types;
using DeepLinkR.Core.Types.EventArgs;
using Moq;
using WK.Libraries.SharpClipboardNS;
using Xunit;

namespace DeepLinkR.Core.Tests.ServiceTests
{
	public class ClipboardManagerTests
	{
		[Fact]
		public void TextIsCopiedToClipboard()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var textCopyMapper = Mock.Get((ITextCopyMapper)mockObjects[nameof(ITextCopyMapper)]);

			var clipboardManager = MockFactories.ClipboardManagerFactory(mockObjects);
			clipboardManager.CopyTextToClipboard("test");

			textCopyMapper.Verify(x => x.SetText(It.IsAny<string>()), Times.Once);
		}

		[Fact]
		public void ClipBoardUpdateEventsAreSubscribed()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var clipboardConfig = Mock.Get((IClipboardConfiguration)mockObjects[nameof(IClipboardConfiguration)]);
			var sharpClipboardMapper = Mock.Get((ISharpClipboardMapper)mockObjects[nameof(ISharpClipboardMapper)]);
			var textCopyMapper = Mock.Get((ITextCopyMapper)mockObjects[nameof(ITextCopyMapper)]);

			var clipboardManager = MockFactories.ClipboardManagerFactory(mockObjects);
			var eventRaised = false;
			clipboardManager.ClipboardTextUpdateReceived += (s, e) =>
			{
				eventRaised = true;
			};
			sharpClipboardMapper.Raise(
				x => x.ClipboardChanged += null,
				this,
				new ClipboardChangedEventArgs(
					applicationName: "testApp",
					clipboardContent: "testContent",
					contentType: SharpClipboard.ContentTypes.Text));

			Assert.True(eventRaised);
		}

		[Fact]
		public void ClipboardEventsAreNotFiredWhenContentIsNotText()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var clipboardConfig = Mock.Get((IClipboardConfiguration)mockObjects[nameof(IClipboardConfiguration)]);
			var sharpClipboardMapper = Mock.Get((ISharpClipboardMapper)mockObjects[nameof(ISharpClipboardMapper)]);
			var textCopyMapper = Mock.Get((ITextCopyMapper)mockObjects[nameof(ITextCopyMapper)]);

			var clipboardManager = MockFactories.ClipboardManagerFactory(mockObjects);
			var eventRaised = false;
			clipboardManager.ClipboardTextUpdateReceived += (s, e) => { eventRaised = true; };
			sharpClipboardMapper.Raise(
				x => x.ClipboardChanged += null,
				this,
				new ClipboardChangedEventArgs(
					applicationName: "testApp",
					clipboardContent: "testContent",
					contentType: SharpClipboard.ContentTypes.Other));

			Assert.False(eventRaised);
		}

		[Fact]
		public void ClipboardEventsAreNotFiredApplicationNameIsEqualToThisApplicationName()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var clipboardConfig = Mock.Get((IClipboardConfiguration)mockObjects[nameof(IClipboardConfiguration)]);
			var sharpClipboardMapper = Mock.Get((ISharpClipboardMapper)mockObjects[nameof(ISharpClipboardMapper)]);
			var textCopyMapper = Mock.Get((ITextCopyMapper)mockObjects[nameof(ITextCopyMapper)]);

			var clipboardManager = MockFactories.ClipboardManagerFactory(mockObjects);
			clipboardManager.AppName = "testApp";
			var eventRaised = false;
			clipboardManager.ClipboardTextUpdateReceived += (s, e) => { eventRaised = true; };
			sharpClipboardMapper.Raise(
				x => x.ClipboardChanged += null,
				this,
				new ClipboardChangedEventArgs(
					applicationName: "testApp",
					clipboardContent: "testContent",
					contentType: SharpClipboard.ContentTypes.Text));

			Assert.False(eventRaised);
		}

		[Theory]
		[InlineData("testText\r\ntest", false, 0, 1)]
		[InlineData("testText\r\ntest", true, 10, 2)]
		[InlineData("testText\r\ntest", true, 1, 1)]
		public void CorrectNumberOfEntriesIsPublished(string testText, bool multipleRows, int maxMultipleRows, int expectedEntriesCount)
		{
			var mockObjects = MockFactories.GetMockObjects();
			var clipboardConfig = Mock.Get((IClipboardConfiguration)mockObjects[nameof(IClipboardConfiguration)]);
			var sharpClipboardMapper = Mock.Get((ISharpClipboardMapper)mockObjects[nameof(ISharpClipboardMapper)]);
			var textCopyMapper = Mock.Get((ITextCopyMapper)mockObjects[nameof(ITextCopyMapper)]);

			clipboardConfig.SetupGet(x => x.MaxProcessedRows).Returns(maxMultipleRows);
			clipboardConfig.SetupGet(x => x.ProcessMultipleRows).Returns(multipleRows);

			var clipboardManager = MockFactories.ClipboardManagerFactory(mockObjects);
			clipboardManager.AppName = "testApp2";
			var eventCorrectRaised = false;
			clipboardManager.ClipboardTextUpdateReceived += (s, e) =>
			{
				eventCorrectRaised = e.ClipboardEntries.Length == expectedEntriesCount;
			};

			sharpClipboardMapper.Raise(
				x => x.ClipboardChanged += null,
				this,
				new ClipboardChangedEventArgs(
					applicationName: "testApp",
					clipboardContent: testText,
					contentType: SharpClipboard.ContentTypes.Text));

			Assert.True(eventCorrectRaised);
		}

		[Fact]
		public void ClipBoardUpdateEventsAreCorrectlyProcessed()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var clipboardConfig = Mock.Get((IClipboardConfiguration)mockObjects[nameof(IClipboardConfiguration)]);
			var sharpClipboardMapper = Mock.Get((ISharpClipboardMapper)mockObjects[nameof(ISharpClipboardMapper)]);
			var textCopyMapper = Mock.Get((ITextCopyMapper)mockObjects[nameof(ITextCopyMapper)]);

			var clipboardManager = MockFactories.ClipboardManagerFactory(mockObjects);

			string[] entries = new[] { string.Empty };
			clipboardManager.ClipboardTextUpdateReceived += (s, e) =>
			{
				entries = e.ClipboardEntries;
			};

			sharpClipboardMapper.Raise(
				x => x.ClipboardChanged += null,
				this,
				new ClipboardChangedEventArgs(
					applicationName: "testApp",
					clipboardContent: "testContent",
					contentType: SharpClipboard.ContentTypes.Text));

			Assert.True(entries.Length == 1);
			Assert.Equal("testContent", entries[0]);
		}

		[Fact]
		public void ProcessingMultipleRowsAndTrimmingSucceeds()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var clipboardConfig = Mock.Get((IClipboardConfiguration)mockObjects[nameof(IClipboardConfiguration)]);
			var sharpClipboardMapper = Mock.Get((ISharpClipboardMapper)mockObjects[nameof(ISharpClipboardMapper)]);
			var textCopyMapper = Mock.Get((ITextCopyMapper)mockObjects[nameof(ITextCopyMapper)]);
			clipboardConfig.SetupGet(x => x.AutomaticTrim).Returns(true);
			clipboardConfig.SetupGet(x => x.ProcessMultipleRows).Returns(true);
			clipboardConfig.SetupGet(x => x.MaxProcessedRows).Returns(100);

			var clipboardManager = MockFactories.ClipboardManagerFactory(mockObjects);

			string[] entries = new[] { string.Empty };
			clipboardManager.ClipboardTextUpdateReceived += (s, e) =>
			{
				entries = e.ClipboardEntries;
			};

			sharpClipboardMapper.Raise(
				x => x.ClipboardChanged += null,
				this,
				new ClipboardChangedEventArgs(
					applicationName: "testApp",
					clipboardContent: " testContent " + Environment.NewLine + " testContent2 ",
					contentType: SharpClipboard.ContentTypes.Text));

			Assert.True(entries.Length == 2);
			Assert.Equal("testContent", entries[0]);
			Assert.Equal("testContent2", entries[1]);
		}
	}
}
