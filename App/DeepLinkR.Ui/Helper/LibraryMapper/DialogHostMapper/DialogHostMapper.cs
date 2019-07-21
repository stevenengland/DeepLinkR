using System.Threading.Tasks;
using DeepLinkR.Ui.ViewModels;
using DeepLinkR.Ui.Views;
using MaterialDesignThemes.Wpf;

namespace DeepLinkR.Ui.Helper.LibraryMapper.DialogHostMapper
{
    public class DialogHostMapper : IDialogHostMapper
    {
	    public Task<object> Show(object content, object dialogIdentifier)
	    {
		    return DialogHost.Show(content, dialogIdentifier);
	    }

	    public ErrorView GetErrorView(string errorMessage)
	    {
		    return new ErrorView()
		    {
				DataContext = new ErrorViewModel()
				{
					ErrorMessage = errorMessage,
				},
		    };
	    }
	}
}
