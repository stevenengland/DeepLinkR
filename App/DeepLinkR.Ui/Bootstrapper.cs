using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using Caliburn.Micro;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Helper.LibraryMapper.SharpClipboardMapper;
using DeepLinkR.Core.Helper.LibraryMapper.TextCopyMapper;
using DeepLinkR.Core.Services.BrowserManager;
using DeepLinkR.Core.Services.ClipboardManager;
using DeepLinkR.Core.Services.DeepLinkManager;
using DeepLinkR.Core.Services.ProcessProxy;
using DeepLinkR.Core.Types;
using DeepLinkR.Ui.Events;
using DeepLinkR.Ui.Helper.LibraryMapper.DialogHostMapper;
using DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper;
using DeepLinkR.Ui.Models;
using DeepLinkR.Ui.ViewModels;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;

namespace DeepLinkR.Ui
{
	public class Bootstrapper : BootstrapperBase
	{
		private readonly SimpleContainer simpleContainer = new SimpleContainer();

		public Bootstrapper()
		{
			this.Initialize();
		}

		private IConfigurationCollection ConfigurationCollection { get; set; }

		private IDeepLinkManager DeepLinkManager { get; set; }

		private IClipboardManager ClipboardManager { get; set; }

		private ISnackbarMessageQueue SbMessageQueue { get; set; }

		protected override void Configure()
		{
			// base.Configure();
			this.ConfigurationCollection = this.ReadConfiguration();

			this.DeepLinkManager = new DeepLinkManager(this.ConfigurationCollection.DeepLinkConfiguration);

			this.ClipboardManager = new ClipboardManager(
				this.ConfigurationCollection.AppConfiguration.ClipboardConfiguration,
				new SharpClipboardMapper(),
				new TextCopyMapper());

			var mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<DeepLinkMatch, DeepLinkMatchDisplayModel>();
			});

			var autoMapper = mapperConfiguration.CreateMapper();

			this.SbMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));

			this.simpleContainer.Instance(autoMapper);

			this.simpleContainer.Instance(this.simpleContainer);

				// .PerRequest
			this.simpleContainer
				.Singleton<IWindowManager, WindowManager>()
				.Singleton<IEventAggregator, EventAggregator>()
				.Singleton<INHotkeyManagerMapper, NHotkeyManagerMapper>()
				.Instance<IConfigurationCollection>(this.ConfigurationCollection)
				.Instance<IClipboardManager>(this.ClipboardManager)
				.Instance<IDeepLinkManager>(this.DeepLinkManager)
				.Instance<ISnackbarMessageQueue>(this.SbMessageQueue)
				.Instance<IBrowserManager>(new BrowserManager(this.ConfigurationCollection.AppConfiguration.BrowserConfiguration, new ProcessProxy()))
				.PerRequest<IDialogHostMapper, DialogHostMapper>();

			// Registers every ViewModel to the container
			this.GetType().Assembly.GetTypes()
				.Where(type => type.IsClass)
				.Where(type => type.Name.EndsWith("ViewModel"))
				.ToList()
				.ForEach(viewModelType => this.simpleContainer.RegisterPerRequest(
					viewModelType, viewModelType.ToString(), viewModelType));

			// Alternative if handling singleton vs. perRequest is needed
			// container
			// .Singleton<MainPageViewModel>()
			// .Singleton<MainViewModel>()
			// .Singleton<AccountsViewModel>()
			// .Singleton<AboutViewModel>()
			// .PerRequest<SearchViewModel>()
			// .PerRequest<SearchResultViewModel>()
			// .PerRequest<CheckinViewModel>();
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			// base.OnStartup(sender, e);
			this.DisplayRootViewFor<ShellViewModel>();
		}

		protected override object GetInstance(Type service, string key)
		{
			return this.simpleContainer.GetInstance(service, key);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return this.simpleContainer.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			this.simpleContainer.BuildUp(instance);
		}

		protected override void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			// Todo: Write you custom code for handling Global unhandled excpetion of Dispatcher or UI thread.
			// base.OnUnhandledException(sender, e);

			// DialogHost.Show()
			// MessageBox.Show(e.Exception.Message, "An error as occurred", MessageBoxButton.OK);
			var eventAggregator = (IEventAggregator)this.simpleContainer.GetInstance(typeof(IEventAggregator), null);
			eventAggregator.PublishOnUIThread(new ErrorEvent(e.Exception, "An unhandled exception occured: " + e.Exception.Message, true));
			e.Handled = true;
		}

		private ConfigurationCollection ReadConfiguration()
		{
			using (var streamReader = new StreamReader("config.json"))
			{
				var jsonString = streamReader.ReadToEnd();
				return JsonConvert.DeserializeObject<ConfigurationCollection>(jsonString);
			}
		}
	}
}
