using System;

using EasySoftware.MvvmMini.Core;

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

		public IClosableViewModel CreateDocumentViewModel()
		{
			return this._container.Resolve<IClosableViewModel>(ViewModels.Document);
		}

      public IWindowViewModel CreateMainViewModel()
      {
			return this._container.Resolve<IWindowViewModel>(ViewModels.Main);
      }
   }
}
