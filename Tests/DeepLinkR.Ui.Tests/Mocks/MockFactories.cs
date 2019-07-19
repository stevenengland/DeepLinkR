using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Caliburn.Micro;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Services.ClipboardManager;
using DeepLinkR.Core.Services.DeepLinkManager;
using DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper;
using MaterialDesignThemes.Wpf;
using Moq;

namespace DeepLinkR.Ui.Tests.Mocks
{
	public static class MockFactories
	{
		public static IDeepLinkManager GetDeepLinkManager()
		{
			return new Mock<IDeepLinkManager>().Object;
		}

		public static IConfigurationCollection GetConfigurationCollection()
		{
			return new Mock<IConfigurationCollection>().Object;
		}

		public static IClipboardManager GetClipboardManager()
		{
			return new Mock<IClipboardManager>().Object;
		}

		public static IMapper GetMapper()
		{
			return new Mock<IMapper>().Object;
		}

		public static IEventAggregator GetEventAggregator()
		{
			return new Mock<IEventAggregator>().Object;
		}

		public static ISnackbarMessageQueue GetISnackbarMessageQueue()
		{
			return new Mock<ISnackbarMessageQueue>().Object;
		}

		public static INHotkeyManagerMapper GetINHotkeyManagerMapper()
		{
			return new Mock<INHotkeyManagerMapper>().Object;
		}
	}
}
