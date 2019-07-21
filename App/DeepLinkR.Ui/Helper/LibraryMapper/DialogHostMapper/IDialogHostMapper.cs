using System.Threading.Tasks;
using DeepLinkR.Ui.Views;

namespace DeepLinkR.Ui.Helper.LibraryMapper.DialogHostMapper
{
    public interface IDialogHostMapper
    {
	    Task<object> Show(object content, object dialogIdentifier);

	    ErrorView GetErrorView(string errorMessage);
    }
}
