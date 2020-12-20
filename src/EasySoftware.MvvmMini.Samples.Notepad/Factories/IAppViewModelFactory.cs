﻿using System.Windows;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;

namespace EasySoftware.MvvmMini.Samples.Notepad.Factories
{
	public interface IAppViewModelFactory : IViewModelFactory
	{
		IMessageBoxDialog CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons);
	}
}