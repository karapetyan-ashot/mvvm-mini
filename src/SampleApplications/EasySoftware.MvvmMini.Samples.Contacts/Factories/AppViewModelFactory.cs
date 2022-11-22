using System;
using System.Windows;

using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts.Factories
{
    public class AppViewModelFactory : IAppViewModelFactory
    {
        private readonly IServiceProvider _provider;

        public AppViewModelFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IMessageBoxViewModel CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons)
        {
            return _provider.GetViewModel<IMessageBoxViewModel>(message, title, buttons);
        }

        public IMainViewModel CreateMainViewModel()
        {
            return _provider.GetViewModel<IMainViewModel>();
        }

        public ILoginViewModel CreateLoginDialog()
        {
            return _provider.GetViewModel<ILoginViewModel>();
        }

        public IContactEditorViewModel CreateContactEditorDialog(ContactModel contact)
        {
            return _provider.GetViewModel<IContactEditorViewModel>(contact);
        }
    }
}