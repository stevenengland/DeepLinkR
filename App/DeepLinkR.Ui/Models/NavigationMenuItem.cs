﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DeepLinkR.Ui.Annotations;
using DeepLinkR.Ui.Helper.Extensions;

namespace DeepLinkR.Ui.Models
{
	public class NavigationMenuItem : INotifyPropertyChanged
	{
		private string name;
		private object content;
		private ScrollBarVisibility horizontalScrollBarVisibilityRequirement;
		private ScrollBarVisibility verticalScrollBarVisibilityRequirement;
		private Thickness marginRequirement = new Thickness(16);

		public event PropertyChangedEventHandler PropertyChanged;

		public string Name
		{
			get => this.name;
			set => this.MutateVerbose(ref this.name, value, this.RaisePropertyChanged());
		}

		public object Content
		{
			get => this.content;
			set => this.MutateVerbose(ref this.content, value, this.RaisePropertyChanged());
		}

		public ScrollBarVisibility HorizontalScrollBarVisibilityRequirement
		{
			get => this.horizontalScrollBarVisibilityRequirement;
			set => this.MutateVerbose(ref this.horizontalScrollBarVisibilityRequirement, value, this.RaisePropertyChanged());
		}

		public ScrollBarVisibility VerticalScrollBarVisibilityRequirement
		{
			get => this.verticalScrollBarVisibilityRequirement;
			set => this.MutateVerbose(ref this.verticalScrollBarVisibilityRequirement, value, this.RaisePropertyChanged());
		}

		public Thickness MarginRequirement
		{
			get => this.marginRequirement;
			set => this.MutateVerbose(ref this.marginRequirement, value, this.RaisePropertyChanged());
		}

		private Action<PropertyChangedEventArgs> RaisePropertyChanged()
		{
			return args => this.PropertyChanged?.Invoke(this, args);
		}
	}
}
