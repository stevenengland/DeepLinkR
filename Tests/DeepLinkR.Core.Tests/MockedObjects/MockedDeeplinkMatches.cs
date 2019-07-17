using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types;

namespace DeepLinkR.Core.Tests.MockedObjects
{
	public static class MockedDeeplinkMatches
	{
		public static DeepLinkMatch SimpleDeepLinkMatch { get; set; } = new DeepLinkMatch()
		{
			DeepLinkName = "dlName", DeepLinkCategoryName = "dlCName", DeepLinkUrl = "dlUrl", DeepLinkKeyName = "dlKName",
		};

		public static List<DeepLinkMatch> OrderTestList { get; set; } = new List<DeepLinkMatch>()
		{
			new DeepLinkMatch() { DeepLinkCategoryName = "Cat2", DeepLinkKeyName = "Key3", DeepLinkName = "Name_2.3.1", DeepLinkUrl = "Url_2.3.1", },
			new DeepLinkMatch() { DeepLinkCategoryName = "Cat1", DeepLinkKeyName = "Key1", DeepLinkName = "Name_1.1.1", DeepLinkUrl = "Url_1.1.1", },
			new DeepLinkMatch() { DeepLinkCategoryName = "Cat1", DeepLinkKeyName = "Key2", DeepLinkName = "Name_1.2.1", DeepLinkUrl = "Url_1.2.1", },
			new DeepLinkMatch() { DeepLinkCategoryName = "Cat1", DeepLinkKeyName = "Key2", DeepLinkName = "Name_1.2.2", DeepLinkUrl = "Url_1.2.2", },
			new DeepLinkMatch() { DeepLinkCategoryName = "Cat1", DeepLinkKeyName = "Key3", DeepLinkName = "Name_1.3.1", DeepLinkUrl = "Url_1.3.1", },
			new DeepLinkMatch() { DeepLinkCategoryName = "Cat2", DeepLinkKeyName = "Key1", DeepLinkName = "Name_2.1.1", DeepLinkUrl = "Url_2.1.1", },
			new DeepLinkMatch() { DeepLinkCategoryName = "Cat2", DeepLinkKeyName = "Key1", DeepLinkName = "Name_2.1.2", DeepLinkUrl = "Url_2.1.2", },
			new DeepLinkMatch() { DeepLinkCategoryName = "Cat2", DeepLinkKeyName = "Key2", DeepLinkName = "Name_2.2.1", DeepLinkUrl = "Url_2.2.1", },
			new DeepLinkMatch() { DeepLinkCategoryName = "Cat1", DeepLinkKeyName = "Key2", DeepLinkName = "Name_1.2.3", DeepLinkUrl = "Url_1.2.3", },
		};
	}
}
