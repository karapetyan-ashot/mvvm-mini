using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Notepad.Workplaces.Document;

namespace EasySoftware.MvvmMini.Samples.Notepad
{
	public class MainViewModel : WindowViewModelBase, IMainViewModel
	{
		private int docNum = 0;
		private readonly IServiceProvider _serviceProvider;

		public MainViewModel(IViewAdapter viewAdapter, IServiceProvider serviceProvider) : base(viewAdapter)
		{
			_serviceProvider = serviceProvider;
			this.Title = "Notepad ++-";

			this.Documents = new ObservableCollection<IDocumentViewModel>();

			this.NewDocumentCommand = new RelayCommand(this.CreateNewDocument);
			this.SaveCommand = new RelayCommand(this.Save, this.CanSave);
		}

		public IRelayCommand NewDocumentCommand { get; }
		public IRelayCommand SaveCommand { get; }

		private ObservableCollection<IDocumentViewModel> _documents;
		public ObservableCollection<IDocumentViewModel> Documents
		{
			get => this._documents;
			set => SetProperty(ref this._documents, value);
		}

		private IDocumentViewModel _currentDocument;
		public IDocumentViewModel CurrentDocument
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
			var doc = _serviceProvider.GetViewModel<IDocumentViewModel>();
			doc.Title = $"New doc {++docNum}";
			doc.Closed += (s, e) =>
			{
				if (s is IDocumentViewModel viewModel)
				{
					if (this.Documents.Contains(viewModel))
						this.Documents.Remove(viewModel);
				}
			};
			this.Documents.Add(doc);
			this.CurrentDocument = doc;
			return Task.CompletedTask;
		}

		private Task Save()
		{
			foreach (var document in this.Documents)
			{
				document.SaveCommand.Execute(null);
			}
			return Task.CompletedTask;
		}

		private bool CanSave()
		{
			return this.Documents.Any(x => x.SaveCommand.CanExecute(null) == true);
		}

		public override async Task OnClosing(CancelEventArgs e)
		{
			var currDoc = this.CurrentDocument;
			foreach (var document in this.Documents.ToList())
			{
				this.CurrentDocument = document;
				await ((IRelayCommand)document.CloseCommand).ExecuteAsync(null);
			}

			if (this.Documents.Any())
			{
				e.Cancel = true;
				if (this.Documents.Contains(currDoc))
					this.CurrentDocument = currDoc;
			}
		}
	}
}
