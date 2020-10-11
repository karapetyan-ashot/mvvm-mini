using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;

using Unity;
using Unity.Resolution;

namespace EasySoftware.MvvmMini.Samples.Notepad.Factories
{
	public interface IDialogFactory
	{
		IMessageBoxDialog CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons);
	}

	
}
