using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Caliburn.Micro;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Services.ClipboardManager;
using DeepLinkR.Core.Services.DeepLinkManager;
using DeepLinkR.Core.Types;
using DeepLinkR.Core.Types.EventArgs;
using DeepLinkR.Ui.Models;

namespace DeepLinkR.Ui.ViewModels
{
	public class DeepLinkCollectionViewModel : Screen //todo andere sicht
	{
		private IConfigurationCollection configurationCollection;
		private IClipboardManager clipboardManager;
		private IDeepLinkManager deepLinkManager;
		private IMapper mapper;
		private BindingList<DeepLinkMatchDisplayModel> deepLinkMatchesDisplayModels;

		public DeepLinkCollectionViewModel(IConfigurationCollection configurationCollection, IClipboardManager clipboardManager, IDeepLinkManager deepLinkManager, IMapper mapper)
		{
			this.clipboardManager = clipboardManager;
			this.configurationCollection = configurationCollection;
			this.deepLinkManager = deepLinkManager;
			this.mapper = mapper;

			this.clipboardManager.ClipboardTextUpdateReceived += this.OnClipboardTextUpdateReceived;
		}

		public BindingList<DeepLinkMatchDisplayModel> DeepLinkMatchesDisplayModels
		{
			get => this.deepLinkMatchesDisplayModels;
			set
			{
				this.deepLinkMatchesDisplayModels = value;
				this.NotifyOfPropertyChange(() => this.DeepLinkMatchesDisplayModels);
			}
		}

		private void OnClipboardTextUpdateReceived(object sender, ClipboardTextUpdateEventArgs e)
		{
			var deepLinkMatches = this.deepLinkManager.GetDeepLinkMatches(e.ClipboardText);
			if (deepLinkMatches?.Count >= 0)
			{
				this.DeepLinkMatchesDisplayModels = new BindingList<DeepLinkMatchDisplayModel>(this.mapper.Map<List<DeepLinkMatchDisplayModel>>(deepLinkMatches));
			}
		}

		// event Aggregation for history
	}
}
