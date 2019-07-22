using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Services.BrowserManager;
using DeepLinkR.Core.Services.ProcessProxy;
using DeepLinkR.Core.Tests.MockedObjects;
using DeepLinkR.Core.Tests.Mocks;
using Moq;
using Xunit;

namespace DeepLinkR.Core.Tests.ServiceTests
{
	public class BrowserManagerTests
	{
		[Fact]
		public void DefaultBrowserIsSelected()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var browserConfig = Mock.Get((IBrowserConfiguration)mockObjects[nameof(IBrowserConfiguration)]);
			browserConfig.SetupGet(x => x.BrowserDefinitions).Returns(MockedBrowserDefinitions.BrowserDefinitions);

			var browserManager = MockFactories.BrowserManagerFactory(mockObjects);

			Assert.True(browserManager.DefaultBrowserDefinition.IsDefault);
			Assert.True(browserManager.DefaultBrowserDefinition.Name == "test");
		}

		[Fact]
		public void DefaultBrowserIsSelectedEvenWhenNoBrowserIsSetAsDefault()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var browserConfig = Mock.Get((IBrowserConfiguration)mockObjects[nameof(IBrowserConfiguration)]);
			browserConfig.SetupGet(x => x.BrowserDefinitions).Returns(MockedBrowserDefinitions.BrowserDefinitionsWithoutDefaultBrowser);

			var browserManager = MockFactories.BrowserManagerFactory(mockObjects);

			Assert.False(browserManager.DefaultBrowserDefinition.IsDefault);
			Assert.True(browserManager.DefaultBrowserDefinition.Name == "test");
		}

		[Fact]
		public void ArgumentConstructionSucceeds()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var browserConfig = Mock.Get((IBrowserConfiguration)mockObjects[nameof(IBrowserConfiguration)]);
			browserConfig.SetupGet(x => x.BrowserDefinitions).Returns(MockedBrowserDefinitions.BrowserDefinitions);

			var browserManager = MockFactories.BrowserManagerFactory(mockObjects);

			var url = "https://steven-england.info";
			var result = browserManager.ConstructArguments("-test", url);

			Assert.Equal(result, "-test " + url);

			result = browserManager.ConstructArguments("-url={url} -test", url);

			Assert.Equal(result, $"-url={url} -test");
		}

		[Fact]
		public void ArgumentConstructionSucceedsWithEmptyArgumentInput()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var browserConfig = Mock.Get((IBrowserConfiguration)mockObjects[nameof(IBrowserConfiguration)]);
			browserConfig.SetupGet(x => x.BrowserDefinitions).Returns(MockedBrowserDefinitions.BrowserDefinitions);

			var browserManager = MockFactories.BrowserManagerFactory(mockObjects);

			var url = "https://steven-england.info";
			var result = browserManager.ConstructArguments(string.Empty, url);

			Assert.Equal(url, result);

			result = browserManager.ConstructArguments(null, url);

			Assert.Equal(url, result);
		}

		[Fact]
		public void UrlOpeningWithDefaultBrowserSucceeds()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var proxyMock = Mock.Get((IProcessProxy)mockObjects[nameof(IProcessProxy)]);
			var browserConfig = Mock.Get((IBrowserConfiguration) mockObjects[nameof(IBrowserConfiguration)]);
			browserConfig.SetupGet(x => x.BrowserDefinitions).Returns(MockedBrowserDefinitions.BrowserDefinitions);

			var browserManager = MockFactories.BrowserManagerFactory(mockObjects);
			browserManager.OpenWithDefaultBrowser("test");

			proxyMock.Verify(x => x.StartProcess(It.IsAny<ProcessStartInfo>()));
		}
	}
}
