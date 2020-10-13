using System;
using System.Windows;

using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

using Unity;
using Unity.Resolution;

namespace EasySoftware.MvvmMini.Samples.Contacts.Factories
{
	public class DialogFactory : IDialogFactory
	{
		private IUnityContainer _unityContainer;

		public DialogFactory(IUnityContainer unityContainer)
		{
			this._unityContainer = unityContainer ?? throw new ArgumentNullException(nameof(unityContainer));
		}

		public IContactEditor CreateContactEditorDialog(Contact contact)
		{
			return this._unityContainer.Resolve<IContactEditor>(ViewModels.Contact, new ResolverOverride[] { new ParameterOverride("contact", contact) }); ;
		}

		public ILoginViewModel CreateLoginDialog()
		{
			return this._unityContainer.Resolve<ILoginViewModel>(ViewModels.Login);
		}

		public IMessageBoxDialog CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons)
		{
			return this._unityContainer.Resolve<IMessageBoxDialog>(ViewModels.MessageBox, new ResolverOverride[]
				{
					new ParameterOverride( "message", message ),
					new ParameterOverride( "title", title),
					new ParameterOverride( "buttons", buttons)
				});
		}
	}
}
