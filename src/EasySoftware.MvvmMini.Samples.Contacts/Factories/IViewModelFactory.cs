using System;
using System.Collections.Generic;
using System.Text;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.Samples.Contacts.Factories
{
	public interface IViewModelFactory
	{
		IWindowViewModel CreateMainViewModel();
	}
}
