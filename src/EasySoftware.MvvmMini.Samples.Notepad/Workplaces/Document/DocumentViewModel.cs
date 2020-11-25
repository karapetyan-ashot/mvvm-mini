using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Notepad.Factories;

namespace EasySoftware.MvvmMini.Samples.Notepad.Workplaces.Document
{
	public class DocumentViewModel : ClosableViewModelBase, IDocumentViewModel
	{
		private bool _saved = true;
		private IViewModelFactory _viewModelFactory;

		public DocumentViewModel(IViewAdapter viewAdapter, IViewModelFactory viewModelFactory) : base(viewAdapter)
		{
			this._viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
			this.SaveCommand = new RelayCommand(this.Save, this.CanSave);
			
			this.Title = "unnamed";
		}

		public ICommand SaveCommand { get; }

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
				IMessageBoxDialog messageBox = this._viewModelFactory.CreateMessageBoxDialog($"do you want to save {this.Title} document", "confirm please", MessageBoxButton.YesNoCancel);
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

		private Task Save()
		{
			this._saved = true;
			return Task.CompletedTask;
		}

		private bool CanSave()
		{
			return !this._saved;
		}
	}
}
