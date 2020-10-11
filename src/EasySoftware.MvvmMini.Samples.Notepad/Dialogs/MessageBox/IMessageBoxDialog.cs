﻿using System.Windows;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox
{
	public interface IMessageBoxDialog : IDialogViewModel
	{
		MessageBoxResult DialogResult { get; }
	}
}
