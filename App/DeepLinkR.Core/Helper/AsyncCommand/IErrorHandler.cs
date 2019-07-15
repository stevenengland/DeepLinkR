using System;

namespace DeepLinkR.Core.Helper.AsyncCommand
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
