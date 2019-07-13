using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Services.DeepLinkManager;
using DeepLinkR.Core.Tests.MockedObjects;
using Moq;
using Xunit;

namespace DeepLinkR.Core.Tests.ServiceTests
{
	public class DeepLinkManagerTests
	{
		private readonly DeepLinkManager deepLinkManager;
		private readonly Mock<IDeepLinkConfiguration> deepLinkConfigurationMock = new Mock<IDeepLinkConfiguration>();

		public DeepLinkManagerTests()
		{
			this.deepLinkManager = new DeepLinkManager(this.deepLinkConfigurationMock.Object);
		}

		[Theory]
		[InlineData("ab", 0)]
		[InlineData("abc", 1)]
		[InlineData("abcdefghij", 1)]
		[InlineData("ABCDEFGHIJ", 1)]
		[InlineData("012345678912", 0)]
		[InlineData("1234567890", 2)]
		public void CorrectListOfMatchingKeysIsGenerated(string text, int expectedKeyCount)
		{
			this.deepLinkConfigurationMock.SetupGet(x => x.DeepLinkKeys).Returns(MockedDeepLinkKeys.TestKeys);

			var result = this.deepLinkManager.GetMatchingKeyNames(text);

			Assert.Equal(expectedKeyCount, result.Count);
		}

		[Theory]
		[InlineData("stevenengland", 1)]
		[InlineData("1234567890", 2)]
		[InlineData("a234567890", 2)]
		[InlineData("abcdefghijklmnopq", 1)]
		public void CorrectListOfMatchingDeepLinksIsGenerated(string text, int expectedKeyCount)
		{
			this.deepLinkConfigurationMock.SetupGet(x => x.DeepLinkKeys).Returns(MockedDeepLinkKeys.CategoryTestKeys);
			this.deepLinkConfigurationMock.SetupGet(x => x.DeepLinkCategories).Returns(MockedDeepLinkCategories.TestCategories);

			var result = this.deepLinkManager.GetDeepLinkMatches(text);

			Assert.Equal(expectedKeyCount, result.Count);
		}
	}
}
