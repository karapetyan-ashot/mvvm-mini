using System;
using System.Windows;

using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;

using Unity;
using Unity.Resolution;

namespace EasySoftware.MvvmMini.Samples.Notepad.Factories
{
	public class DialogFactory : IDialogFactory
	{
		private IUnityContainer _container;

		public DialogFactory(IUnityContainer container)
		{
			this._container = container ?? throw new ArgumentNullException(nameof(container));
		}

		public IMessageBoxDialog CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons)
		{
			return this._container.Resolve<IMessageBoxDialog>(ViewModels.MessageBox, new ResolverOverride[]
				{
					new ParameterOverride( "message", message ),
					new ParameterOverride( "title", title),
					new ParameterOverride( "buttons", buttons)
				});
		}

	}
}
