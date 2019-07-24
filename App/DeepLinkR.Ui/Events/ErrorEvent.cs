using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeepLinkR.Ui.Events
{
    public class ErrorEvent
    {
	    public ErrorEvent(Exception exception, string errorMessage, bool shutdown = false)
	    {
		    this.Exception = exception;
		    this.ErrorMessage = errorMessage;
		    this.ApplicationMustShutdown = shutdown;
	    }

	    public Exception Exception { get; }

	    public string ErrorMessage { get; }

	    public bool ApplicationMustShutdown { get; } = false;
    }
}
