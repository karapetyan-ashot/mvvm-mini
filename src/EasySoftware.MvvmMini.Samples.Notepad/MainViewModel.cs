using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Notepad.Factories;

using Unity;
using Unity.Resolution;

namespace EasySoftware.MvvmMini.Samples.Notepad
{
	public class MainViewModel : WindowViewModelBase
	{
		private bool _saved = true;
		private IDialogFactory _dialogFactory;

		public MainViewModel(IView view, IDialogFactory dialogFactory) : base(view)
		{
			this._dialogFactory = dialogFactory ?? throw new ArgumentNullException(nameof(dialogFactory));
			this.SaveCommand = new RelayCommand(this.Save, this.CanSave);
		}

		public ICommand SaveCommand { get; }

		protected override void Loaded()
		{
			this.Title += " loaded";
		}

		private string _text;
		public string Text
		{
			get { return _text; }
			set
			{
				if (_text != value)
				{
					_text = value;
					RaisePropertyChanged();
					this._saved = false;
				}
			}
		}

		public override void OnClosing(CancelEventArgs e)
		{
			if (!this._saved)
			{
				IMessageBoxDialog messageBox = this._dialogFactory.CreateMessageBoxDialog("do you want to save", "confirm please", MessageBoxButton.YesNoCancel);
				messageBox.ShowDialog();

				switch (messageBox.DialogResult)
				{
					case MessageBoxResult.Yes:
						this._saved = true;
						break;
					case MessageBoxResult.No:
						break;
					case MessageBoxResult.Cancel:
						e.Cancel = true;
						break;
				}
			}
		}

		protected override bool CanClose()
		{
			return true;
		}

		private Task Save()
		{

			ObservableCollection<IViewModel> c = new ObservableCollection<IViewModel>();
			ReadOnlyObservableCollection<IViewModel> roc = new ReadOnlyObservableCollection<IViewModel>(c);

			this._saved = true;
			return Task.CompletedTask;
		}

		private bool CanSave()
		{
			return !this._saved;
		}
	}
}
