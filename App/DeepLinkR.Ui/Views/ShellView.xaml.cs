using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeepLinkR.Ui.Views
{
	/// <summary>
	/// Interaction logic for ShellView.xaml.
	/// </summary>
	public partial class ShellView : Window
	{
		public ShellView()
		{
			this.InitializeComponent();
		}

		private void ColorZone_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				// Fix: https://github.com/stevenengland/DeepLinkR/issues/6
				// If the popup box is not checked the mousemove event gets handled because the popup box is part of the color zone
				if (this.WindowState == System.Windows.WindowState.Maximized && !this.PopupBox.IsPopupOpen)
				{
					var x = e.GetPosition(this).X;
					var y = e.GetPosition(this).Y;
					var widthRatio = x / this.ActualWidth;
					var heightRatio = y / this.TitleBar.ActualHeight;
					this.WindowState = System.Windows.WindowState.Normal;
					this.Left = x - Convert.ToInt32(this.ActualWidth * widthRatio);
					this.Top = y - Convert.ToInt32(this.TitleBar.ActualHeight * heightRatio);
				}

				this.DragMove();
			}
		}
	}
}
