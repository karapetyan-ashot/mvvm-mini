using System.Collections.Generic;
using System.Windows;

using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox;

using Unity;

namespace EasySoftware.MvvmMini.Samples.Contacts.Factories
{
	public class AppViewModelFactory : ViewModelFactoryBase, IAppViewModelFactory
	{

		public AppViewModelFactory(IUnityContainer unityContainer) : base(unityContainer) { }

		public IMessageBoxViewModel CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons)
		{
			return this.ResolveViewModel<IMessageBoxViewModel>(
				(nameof(message), message),
				(nameof(title), title),
				(nameof(buttons), buttons)
			);
		}
	}
}