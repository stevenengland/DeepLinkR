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
using DeepLinkR.Core.Services.LoggerManager;
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
		private ErrorEvent firstErrorEvent;

		public Bootstrapper()
		{
			this.Initialize();
		}

		private IConfigurationCollection ConfigurationCollection { get; set; }

		private IDeepLinkManager DeepLinkManager { get; set; }

		private IClipboardManager ClipboardManager { get; set; }

		private ISnackbarMessageQueue SbMessageQueue { get; set; }

		private IMapper Mapper { get; set; }

		private IBrowserManager BrowserManager { get; set; }

		private ILoggerManager LoggerManager { get; set; }

		protected override void Configure()
		{
			try
			{
				this.ConfigurationCollection = this.ReadConfiguration();
			}
			catch (Exception e)
			{
				this.SetErrorEvent(new ErrorEvent(e, "Configuration could not be loaded: " + e.Message, true));

				// ToDo: Minimal viable fake configuration to load the app
				this.ConfigurationCollection = new ConfigurationCollection(
					new DeepLinkConfiguration(),
					new AppConfiguration(
						new BrowserConfiguration(),
						new ClipboardConfiguration(),
						new LoggingConfiguration()));
			}

			try
			{
				this.ConfigurationCollection.Validate();
				this.simpleContainer.Instance<IConfigurationCollection>(this.ConfigurationCollection);
			}
			catch (Exception e)
			{
				this.SetErrorEvent(new ErrorEvent(e, "Configuration violates one or more rules: " + e.Message, true));
			}

			try
			{
				this.LoggerManager = Core.Services.LoggerManager.LoggerManager.GetLoggingService(this.ConfigurationCollection.AppConfiguration.LoggingConfiguration);
				this.simpleContainer.Instance<ILoggerManager>(this.LoggerManager);
			}
			catch (Exception e)
			{
				this.SetErrorEvent(new ErrorEvent(e, $"Could not load {nameof(this.LoggerManager)}:" + e.Message, true));
			}

			try
			{
				this.DeepLinkManager = new DeepLinkManager(this.ConfigurationCollection.DeepLinkConfiguration);
				this.simpleContainer.Instance<IDeepLinkManager>(this.DeepLinkManager);
			}
			catch (Exception e)
			{
				this.SetErrorEvent(new ErrorEvent(e, $"Could not load {nameof(this.DeepLinkManager)}:" + e.Message, true));
			}

			try
			{
				this.ClipboardManager = new ClipboardManager(
					this.ConfigurationCollection.AppConfiguration.ClipboardConfiguration,
					new SharpClipboardMapper(),
					new TextCopyMapper());
				this.simpleContainer.Instance<IClipboardManager>(this.ClipboardManager);
			}
			catch (Exception e)
			{
				this.SetErrorEvent(new ErrorEvent(e, $"Could not load {nameof(this.ClipboardManager)}:" + e.Message, true));
			}

			try
			{
				var mapperConfiguration = new MapperConfiguration(cfg =>
				{
					cfg.CreateMap<DeepLinkMatch, DeepLinkMatchDisplayModel>();
				});

				this.Mapper = mapperConfiguration.CreateMapper();

				this.simpleContainer.Instance(this.Mapper);
			}
			catch (Exception e)
			{
				this.SetErrorEvent(new ErrorEvent(e, $"Could not load {nameof(this.Mapper)}:" + e.Message, true));
			}

			try
			{
				this.BrowserManager =
					new BrowserManager(
						this.ConfigurationCollection.AppConfiguration.BrowserConfiguration,
						new ProcessProxy());
				this.simpleContainer.Instance<IBrowserManager>(this.BrowserManager);
			}
			catch (Exception e)
			{
				this.SetErrorEvent(new ErrorEvent(e, $"Could not load {nameof(this.BrowserManager)}:" + e.Message, true));
			}

			try
			{
				this.SbMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
				this.simpleContainer.Instance<ISnackbarMessageQueue>(this.SbMessageQueue);
			}
			catch (Exception e)
			{
				this.SetErrorEvent(new ErrorEvent(e, $"Could not load {nameof(this.SbMessageQueue)}:" + e.Message, true));
			}

			// Register itself
			this.simpleContainer.Instance(this.simpleContainer);

			// .PerRequest
			this.simpleContainer
				.Singleton<IWindowManager, WindowManager>()
				.Singleton<IEventAggregator, EventAggregator>()
				.Singleton<INHotkeyManagerMapper, NHotkeyManagerMapper>()
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
			if (this.firstErrorEvent != null)
			{
				var eventAggregator = (IEventAggregator)this.simpleContainer.GetInstance(typeof(IEventAggregator), null);
				eventAggregator.PublishOnUIThread(
					new ErrorEvent(
						this.firstErrorEvent.Exception,
						"An unhandled exception occured: " + this.firstErrorEvent.ErrorMessage,
						this.firstErrorEvent.ApplicationMustShutdown));
			}
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

		private void SetErrorEvent(ErrorEvent errorEvent)
		{
			// only the youngest should be kept
			if (this.firstErrorEvent == null)
			{
				this.firstErrorEvent = errorEvent;
			}
		}
	}
}
