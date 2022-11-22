using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;

namespace EasySoftware.MvvmMini.Samples.Notepad.Workplaces.Document
{
	public class DocumentViewModel : ClosableViewModelBase, IDocumentViewModel
	{
		private bool _saved = true;
		private readonly IServiceProvider _serviceProvider;

		public DocumentViewModel(IViewAdapter viewAdapter,IServiceProvider serviceProvider) : base(viewAdapter)
		{
			_serviceProvider = serviceProvider;
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

		public override async Task OnClosing(CancelEventArgs e)
		{
			if (!this._saved)
			{
                IMessageBoxViewModel messageBox = this._serviceProvider.GetViewModel<IMessageBoxViewModel>($"do you want to save {this.Title} document", "confirm please", MessageBoxButton.YesNoCancel);
                messageBox.ShowDialog();

				switch (messageBox.DialogResult)
				{
					case MessageBoxResult.Yes:
						await Save();						
						break;
					case MessageBoxResult.No:
						break;
					case MessageBoxResult.Cancel:
						e.Cancel = true;
						break;
				}
			}
		}

		private async Task Save()
		{
			await Task.Delay(2000);
			this._saved = true;
		}

		private bool CanSave()
		{
			if (this.HasErrors)
				return false;
			return !this._saved;
		}
	}
}
