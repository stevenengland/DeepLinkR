using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types;

namespace DeepLinkR.Core.Tests.MockedObjects
{
	public static class MockedDeepLinkCategories
	{
		public static List<DeepLinkCategory> TestCategories { get; set; } = new List<DeepLinkCategory>()
		{
			new DeepLinkCategory()
			{
				Name = "Github",
				DeepLinks = new List<DeepLink>()
				{
					new DeepLink()
					{
						Name = "User",
						Url = "https://github.com/{Github User}",
					},
				},
			},
			new DeepLinkCategory()
			{
				Name = "Example Page",
				DeepLinks = new List<DeepLink>()
				{
					new DeepLink()
					{
						Name = "Order",
						Url = "https://example.com/orders/{ExamplePage Order ID}",
					},
					new DeepLink()
					{
						Name = "User",
						Url = "https://example.com/users/{ExamplePage User ID}",
					},
				},
			},
			new DeepLinkCategory()
			{
				Name = "Second Example Page",
				DeepLinks = new List<DeepLink>()
				{
					new DeepLink()
					{
						Name = "Order",
						Url = "https://secondexample.com/orders/{ExamplePage Order ID}",
					},
				},
			},
		};
	}
}
