using System;
using System.Collections.Generic;
using System.Text;

using EasySoftware.MvvmMini.Core;

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
			return this._unityContainer.Resolve<IWindowViewModel>(ViewModels.Main);
		}
	}
}
