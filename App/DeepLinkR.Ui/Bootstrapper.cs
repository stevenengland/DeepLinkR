using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using Caliburn.Micro;
using DeepLinkR.Core.Services.ClipboardManager;
using DeepLinkR.Ui.Helper.LibraryMapper.NHotkeyManagerMapper;
using DeepLinkR.Ui.ViewModels;

namespace DeepLinkR.Ui
{
	public class Bootstrapper : BootstrapperBase
	{
		private SimpleContainer simpleContainer = new SimpleContainer();

		public Bootstrapper()
		{
			this.Initialize();
		}

		protected override void Configure()
		{
			// base.Configure();

			var mapperConfiguration = new MapperConfiguration(cfg =>
			{
				// cfg.CreateMap<libModel, uiModel>();
			});

			var autoMapper = mapperConfiguration.CreateMapper();

			this.simpleContainer.Instance(autoMapper);

			this.simpleContainer.Instance(this.simpleContainer);
				// .PerRequest

			this.simpleContainer
				.Singleton<IWindowManager, WindowManager>()
				.Singleton<IEventAggregator, EventAggregator>()
				.Singleton<IClipboardManager, ClipboardManager>()
				.Singleton<INHotkeyManagerMapper, NHotkeyManagerMapper>();

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
	}
}
