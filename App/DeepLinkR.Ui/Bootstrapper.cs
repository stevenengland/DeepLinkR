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
using DeepLinkR.Core.Services.ClipboardManager;
using DeepLinkR.Core.Services.DeepLinkManager;
using DeepLinkR.Core.Types;
using DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper;
using DeepLinkR.Ui.Models;
using DeepLinkR.Ui.ViewModels;
using Newtonsoft.Json;

namespace DeepLinkR.Ui
{
	public class Bootstrapper : BootstrapperBase
	{
		private readonly SimpleContainer simpleContainer = new SimpleContainer();

		private IConfigurationCollection ConfigurationCollection { get; set; }

		private IDeepLinkManager DeepLinkManager { get; set; }

		public Bootstrapper()
		{
			this.Initialize();
		}

		protected override void Configure()
		{
			// base.Configure();
			this.ConfigurationCollection = this.ReadConfiguration();

			this.DeepLinkManager = new DeepLinkManager(this.ConfigurationCollection.DeepLinkConfiguration);

			var mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<DeepLinkMatch, DeepLinkMatchDisplayModel>();
			});

			var autoMapper = mapperConfiguration.CreateMapper();

			this.simpleContainer.Instance(autoMapper);

			this.simpleContainer.Instance(this.simpleContainer);

				// .PerRequest
			this.simpleContainer
				.Singleton<IWindowManager, WindowManager>()
				.Singleton<IEventAggregator, EventAggregator>()
				.Singleton<IClipboardManager, ClipboardManager>()
				.Singleton<INHotkeyManagerMapper, NHotkeyManagerMapper>()
				.Instance<IConfigurationCollection>(this.ConfigurationCollection)
				.Instance<IDeepLinkManager>(this.DeepLinkManager);

			this.GetType().Assembly.GetTypes()
				.Where(type => type.IsClass)
				.Where(type => type.Name.EndsWith("ViewModel"))
				.ToList()
				.ForEach(viewModelType => this.simpleContainer.RegisterPerRequest(
					viewModelType, viewModelType.ToString(), viewModelType));
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
