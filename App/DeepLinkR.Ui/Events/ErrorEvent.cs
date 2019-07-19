using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DeepLinkR.Ui.Events
{
    public class ErrorEvent
    {
	    public ErrorEvent(Exception exception, string errorMessage)
	    {
		    this.Exception = exception;
		    this.ErrorMessage = errorMessage;
	    }

	    public Exception Exception { get; }

	    public string ErrorMessage { get; }
	}
}
