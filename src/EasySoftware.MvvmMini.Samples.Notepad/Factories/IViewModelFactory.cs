﻿using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.Samples.Notepad.Factories
{
	public interface IViewModelFactory
	{
		IClosableViewModel CreateDocumentViewModel();
	}
}
