using System;
using System.Windows;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Notepad.Workplaces.Document;

using Unity;

namespace EasySoftware.MvvmMini.Samples.Notepad.Factories
{
	public class ViewModelFactory : IViewModelFactory
	{
		private IUnityContainer _container;

		public ViewModelFactory(IUnityContainer container)
		{
			this._container = container ?? throw new ArgumentNullException(nameof(container));
		}

		public IMessageBoxDialog CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons)
		{
			IView view = new ViewWrapper(new MessageBoxView());
			return new MessageBoxViewModel(view, message, title, buttons);
		}

		public IClosableViewModel CreateDocumentViewModel()
		{
			IView view = new ViewWrapper(new DocumentView());
			return new DocumentViewModel(view, this);
		}

		public IWindowViewModel CreateMainViewModel()
		{
			IView view = new ViewWrapper(new MainView());
			return new MainViewModel(view, this);
		}
	}
}
