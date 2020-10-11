using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Notepad.Factories;

namespace EasySoftware.MvvmMini.Samples.Notepad
{
	public class MainViewModel : WindowViewModelBase
	{
		private int docNum = 0;
		private IViewModelFactory _viewModelFactory;

		public MainViewModel(IView view, IViewModelFactory viewModelFactory) : base(view)
		{
			this._viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
			
			this.Documents = new ObservableCollection<IClosableViewModel>();

			this.NewDocumentCommand = new RelayCommand(this.CreateNewDocument);
		}

		public ICommand NewDocumentCommand { get; }

		private ObservableCollection<IClosableViewModel> _documents;
		public ObservableCollection<IClosableViewModel> Documents
		{
			get => this._documents;
			set => SetProperty(ref this._documents, value);
		}

		private IClosableViewModel _currentDocument;
		public IClosableViewModel CurrentDocument
		{
			get => this._currentDocument;
			set => SetProperty(ref this._currentDocument, value);
		}

		protected override Task Loaded()
		{
			return CreateNewDocument();			
		}

		private Task CreateNewDocument()
		{
			IClosableViewModel doc = this._viewModelFactory.CreateDocumentViewModel();
			doc.Title = $"New doc {++docNum}";
			doc.Closed += (o, e) =>
			{
				if(o is IClosableViewModel viewModel)
				{
					if (this.Documents.Contains(viewModel))
						this.Documents.Remove(viewModel);
				}
			};
			this.Documents.Add(doc);
			this.CurrentDocument = doc;
			return Task.CompletedTask;
		}

		public override void OnClosing(CancelEventArgs e)
		{
			var currDoc = this.CurrentDocument;
			foreach (var document in this.Documents.ToList())
			{
				this.CurrentDocument = document;
				document.CloseCommand.Execute(null);
			}
			
			if(this.Documents.Any())
			{
				e.Cancel = true;
				if (this.Documents.Contains(currDoc))
					this.CurrentDocument = currDoc;
			}			
			base.OnClosing(e);
		}

	}
}
