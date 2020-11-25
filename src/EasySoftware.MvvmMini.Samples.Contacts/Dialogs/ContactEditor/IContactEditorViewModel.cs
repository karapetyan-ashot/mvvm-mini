using System;
using System.Collections.Generic;
using System.Text;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor
{
	public interface IContactEditorViewModel : IDialogViewModel
	{
		Contact ModifiedContact { get; }
	}
}
