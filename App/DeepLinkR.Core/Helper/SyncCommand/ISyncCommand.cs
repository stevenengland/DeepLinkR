using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeepLinkR.Core.Helper.SyncCommand
{
	public interface ISyncCommand : ICommand
	{
		void ExecuteSync();

		bool CanExecute();
	}

	public interface ISyncCommand<T> : ICommand
	{
		void ExecuteSync(T parameter);

		bool CanExecute(T parameter);
	}
}
