using System;
using System.Threading.Tasks;

namespace DeepLinkR.Core.Helper.AsyncCommand
{
    public static class TaskUtilities
    {
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
#pragma warning disable CA1030 // Use events where appropriate
#pragma warning disable AsyncFixer03 // Avoid fire & forget async void methods
		public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
#pragma warning restore AsyncFixer03 // Avoid fire & forget async void methods
#pragma warning restore CA1030 // Use events where appropriate
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
		{
            try
            {
#pragma warning disable CA2007 // Do not directly await a Task
				await task;
#pragma warning restore CA2007 // Do not directly await a Task
			}
            catch (Exception ex)
            {
                handler?.HandleError(ex);
            }
        }
    }
}
