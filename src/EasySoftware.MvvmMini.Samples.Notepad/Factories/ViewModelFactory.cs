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
			IViewAdapter viewAdapter = new ViewAdapter(new MessageBoxView());
			return new MessageBoxViewModel(viewAdapter, message, title, buttons);
		}

		public IDocumentViewModel CreateDocumentViewModel()
		{
			IViewAdapter viewAdapter = new ViewAdapter(new DocumentView());
			return new DocumentViewModel(viewAdapter, this);
		}

		public IWindowViewModel CreateMainViewModel()
		{
			IViewAdapter viewAdapter = new ViewAdapter(new MainView());
			return new MainViewModel(viewAdapter, this);
		}
	}
}
