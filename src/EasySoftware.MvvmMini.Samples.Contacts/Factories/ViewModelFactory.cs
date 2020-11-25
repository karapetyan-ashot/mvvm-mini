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
			
			IViewAdapter viewAdapter = new ViewAdapter(new MainView());
			return new MainViewModel(viewAdapter, contactsService, this);
		}
		public IContactEditorViewModel CreateContactEditorDialog(Contact contact)
		{
			IViewAdapter viewAdapter = new ViewAdapter(new ContactEditorView());
			return new ContactEditorViewModel(viewAdapter, contact);
		}

		public ILoginViewModel CreateLoginDialog()
		{
			IContactsService contactsService = this._unityContainer.Resolve<IContactsService>();
			
			IViewAdapter viewAdapter = new ViewAdapter(new LoginView());
			return new LoginViewModel(viewAdapter, contactsService);
		}

		public IMessageBoxViewModel CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons)
		{
			IViewAdapter viewAdapter = new ViewAdapter(new MessageBoxView());
			return new MessageBoxViewModel(viewAdapter, message, title, buttons);
		}
	}
}
