using System.Windows;

using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;

using Unity;

namespace EasySoftware.MvvmMini.Samples.Notepad.Factories
{
	public class AppViewModelFactory : ViewModelFactoryBase, IAppViewModelFactory
	{

		public AppViewModelFactory(IUnityContainer unityContainer) : base(unityContainer)
		{
		}

		public IMessageBoxDialog CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons)
		{
			return this.ResolveViewModel<IMessageBoxDialog>(
				(nameof(message), message),
				(nameof(title), title),
				(nameof(buttons), buttons)
			);
		}
	}
}