using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types;

namespace DeepLinkR.Core.Tests.MockedObjects
{
	public static class MockedDeepLinkKeys
	{
		public static List<DeepLinkKey> TestKeys { get; set; } = new List<DeepLinkKey>()
		{
			new DeepLinkKey()
			{
				Name = "String Item",
				RegExPattern = new List<string>()
				{
					@"[a-zA-Z]{3,9}",
					@"[a-z]{10}",
					@"[A-Z]{10}",
				},
			},
			new DeepLinkKey()
			{
				Name = "Number Item",
				RegExPattern = new List<string>()
				{
					@"^\d{10}$",
					@"\d{100}",
				},
			},
			new DeepLinkKey()
			{
				Name = "Number Item 2",
				RegExPattern = new List<string>()
				{
					@"^\d{10}$",
				},
			},
		};

		public static List<DeepLinkKey> CategoryTestKeys { get; set; } = new List<DeepLinkKey>()
		{
			new DeepLinkKey()
			{
				Name = "Github User",
				RegExPattern = new List<string>()
				{
					@"^stevenengland$",
				},
			},
			new DeepLinkKey()
			{
				Name = "ExamplePage Order ID",
				RegExPattern = new List<string>()
				{
					@"^\d{10}$",
					@"^[a-z]\d{9}$",
				},
			},
			new DeepLinkKey()
			{
				Name = "ExamplePage User ID",
				RegExPattern = new List<string>()
				{
					@"^[a-z]{15,20}$",
				},
			},
		};
	}
}
