using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Services.ClipboardManager;
using DeepLinkR.Core.Services.DeepLinkManager;
using DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper;
using MaterialDesignThemes.Wpf;
using Moq;
using Xunit;

namespace DeepLinkR.Ui.Tests
{
	public class BootstrapperTests
	{
		private TestBootstrapper bootstrapper;

		public BootstrapperTests()
		{
			this.bootstrapper = new TestBootstrapper();
		}

		[Fact]
		public void CheckIoCContainerLoadedAllInstancesNeeded()
		{
			var windowManager = this.bootstrapper.GetInstance<IWindowManager>();
			var eventAggregator = this.bootstrapper.GetInstance<IEventAggregator>();
			var clipboardManager = this.bootstrapper.GetInstance<IClipboardManager>();
			var deeplinkManager = this.bootstrapper.GetInstance<IDeepLinkManager>();
			var hotkeyManager = this.bootstrapper.GetInstance<INHotkeyManagerMapper>();
			var configurationCollection = this.bootstrapper.GetInstance<IConfigurationCollection>();
			var snackbarMessageQueue = this.bootstrapper.GetInstance<ISnackbarMessageQueue>();

			Assert.NotNull(windowManager);
			Assert.NotNull(eventAggregator);
			Assert.NotNull(clipboardManager);
			Assert.NotNull(deeplinkManager);
			Assert.NotNull(hotkeyManager);
			Assert.NotNull(configurationCollection);
			Assert.NotNull(snackbarMessageQueue);
		}

		private class TestBootstrapper : Bootstrapper
		{
			public T GetInstance<T>()
			{
				try
				{
					return (T)this.GetInstance(typeof(T), null);
				}
				catch (Exception)
				{
					this.WhatDoIHave();
					throw;
				}
			}

			protected override void StartRuntime()
			{
				this.Configure();
			}

			private void WhatDoIHave()
			{
			}
		}
	}
}
