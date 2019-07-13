using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Ui.Annotations;

namespace DeepLinkR.Ui.Models
{
	public class DeepLinkMatchDisplayModel : INotifyPropertyChanged
	{
		private string deepLinkCategoryName;
		private string deepLingKeyName;
		private string deepLinkName;
		private string deepLinkUrl;

		public event PropertyChangedEventHandler PropertyChanged;

		public string DeepLinkCategoryName
		{
			get => this.deepLinkCategoryName;
			set
			{
				if (value == this.deepLinkCategoryName)
				{
					return;
				}

				this.deepLinkCategoryName = value;
				this.OnPropertyChanged();
			}
		}

		public string DeepLingKeyName
		{
			get => this.deepLingKeyName;
			set
			{
				if (value == this.deepLingKeyName)
				{
					return;
				}

				this.deepLingKeyName = value;
				this.OnPropertyChanged();
			}
		}

		public string DeepLinkName
		{
			get => this.deepLinkName;
			set
			{
				if (value == this.deepLinkName)
				{
					return;
				}

				this.deepLinkName = value;
				this.OnPropertyChanged();
			}
		}

		public string DeepLinkUrl
		{
			get => this.deepLinkUrl;
			set
			{
				if (value == this.deepLinkUrl)
				{
					return;
				}

				this.deepLinkUrl = value;
				this.OnPropertyChanged();
			}
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}	
