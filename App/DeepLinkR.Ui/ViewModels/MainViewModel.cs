using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace DeepLinkR.Ui.ViewModels
{
	public class MainViewModel : Screen
	{
		public MainViewModel(
			DeepLinkCollectionViewModel deepLinkCollectionViewModel,
			DeepLinkHistoryViewModel deepLinkHistoryViewModel)
		{
			this.DeepLinkCollectionViewModel = deepLinkCollectionViewModel;
			this.DeepLinkHistoryViewModel = deepLinkHistoryViewModel;
		}

		public DeepLinkCollectionViewModel DeepLinkCollectionViewModel { get; private set; }

		public DeepLinkHistoryViewModel DeepLinkHistoryViewModel { get; private set; }
	}
}
