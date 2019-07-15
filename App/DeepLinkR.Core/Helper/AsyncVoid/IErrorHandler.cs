using System;

namespace DeepLinkR.Core.Helper.AsyncVoid
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
