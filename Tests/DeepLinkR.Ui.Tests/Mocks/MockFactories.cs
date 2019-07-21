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
using DeepLinkR.Ui.Helper.LibraryMapper.DialogHostMapper;
using DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper;
using DeepLinkR.Ui.ViewModels;
using MaterialDesignThemes.Wpf;
using Moq;

namespace DeepLinkR.Ui.Tests.Mocks
{
	public static class MockFactories
	{
		public static ShellViewModel ShellViewModelFactory(Dictionary<string, object> mockObjects)
		{
			return new ShellViewModel(
				(IClipboardManager)mockObjects[nameof(IClipboardManager)],
				(INHotkeyManagerMapper)mockObjects[nameof(INHotkeyManagerMapper)],
				(ISnackbarMessageQueue)mockObjects[nameof(ISnackbarMessageQueue)],
				(IEventAggregator)mockObjects[nameof(IEventAggregator)],
				(IDialogHostMapper)mockObjects[nameof(IDialogHostMapper)],
				new MainViewModel( // ToDo Factorize
					MockFactories.DeepLinkCollectionViewModelFactory(GetMockObjects()),
					MockFactories.DeepLinkHistoryViewModelFactory(GetMockObjects())),
				new AboutViewModel()
			);
		}

		public static DeepLinkCollectionViewModel DeepLinkCollectionViewModelFactory(Dictionary<string, object> mockObjects)
		{
			return new DeepLinkCollectionViewModel(
				(IConfigurationCollection)mockObjects[nameof(IConfigurationCollection)],
				(IClipboardManager)mockObjects[nameof(IClipboardManager)],
				(IDeepLinkManager)mockObjects[nameof(IDeepLinkManager)],
				(IMapper)mockObjects[nameof(IMapper)],
				(IEventAggregator)mockObjects[nameof(IEventAggregator)]);
		}

		public static DeepLinkHistoryViewModel DeepLinkHistoryViewModelFactory(Dictionary<string, object> mockObjects)
		{
			return new DeepLinkHistoryViewModel((IEventAggregator)mockObjects[nameof(IEventAggregator)]);
		}

		public static Dictionary<string, object> GetMockObjects()
		{
			return new Dictionary<string, object>()
			{
				{ nameof(IConfigurationCollection), MockFactories.GetConfigurationCollection() },
				{ nameof(IClipboardManager), MockFactories.GetClipboardManager() },
				{ nameof(IDeepLinkManager), MockFactories.GetDeepLinkManager() },
				{ nameof(IMapper), MockFactories.GetMapper() },
				{ nameof(IEventAggregator), MockFactories.GetEventAggregator() },
				{ nameof(INHotkeyManagerMapper), MockFactories.GetINHotkeyManagerMapper() },
				{ nameof(ISnackbarMessageQueue), MockFactories.GetISnackbarMessageQueue() },
				{ nameof(IDialogHostMapper), MockFactories.GetIDialogHostMapper() },
			};
		}

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

		public static IDialogHostMapper GetIDialogHostMapper()
		{
			return new Mock<IDialogHostMapper>().Object;
		}
	}
}
