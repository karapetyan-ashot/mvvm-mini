using System;
using System.Windows;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

using Unity;

namespace EasySoftware.MvvmMini.Samples.Contacts.Factories
{
	public class ViewModelFactory : IViewModelFactory
	{
		private IUnityContainer _unityContainer;

		public ViewModelFactory(IUnityContainer unityContainer)
		{
			this._unityContainer = unityContainer ?? throw new ArgumentNullException(nameof(unityContainer));
		}

		public IWindowViewModel CreateMainViewModel()
		{
			IContactsService contactsService = this._unityContainer.Resolve<IContactsService>();
			IViewModelFactory viewModelFactory = this._unityContainer.Resolve<IViewModelFactory>();
			
			IView view = new ViewWrapper(new MainView());
			return new MainViewModel(view, contactsService, viewModelFactory);
		}
		public IContactEditor CreateContactEditorDialog(Contact contact)
		{
			IView view = new ViewWrapper(new ContactEditorView());
			return new ContactEditorViewModel(view, contact);
		}

		public ILoginViewModel CreateLoginDialog()
		{
			IContactsService contactsService = this._unityContainer.Resolve<IContactsService>();
			
			IView view = new ViewWrapper(new LoginView());
			return new LoginViewModel(view, contactsService);
		}

		public IMessageBoxDialog CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons)
		{
			IView view = new ViewWrapper(new MessageBoxView());
			return new MessageBoxViewModel(view, message, title, buttons);
		}
	}
}
