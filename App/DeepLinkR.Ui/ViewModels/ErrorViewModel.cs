using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace DeepLinkR.Ui.ViewModels
{
    public class ErrorViewModel : Screen
    {
	    private string errorMessage;

	    public ErrorViewModel()
	    {
	    }

	    public string ErrorMessage
	    {
		    get => this.errorMessage;
		    set
		    {
			    this.errorMessage = value;
			    this.NotifyOfPropertyChange(() => this.ErrorMessage);
		    }
		}
    }
}
