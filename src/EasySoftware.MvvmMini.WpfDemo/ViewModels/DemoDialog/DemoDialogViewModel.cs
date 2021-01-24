using System.Threading.Tasks;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.WpfDemo.ViewModels.DemoDialog
{
	public interface IDemoDialogViewModel : IDialogViewModel
	{
		string ResultMessage { get; }
	}

	public class DemoDialogViewModel : DialogViewModelBase, IDemoDialogViewModel
	{
		public DemoDialogViewModel(IViewAdapter viewAdapter) : base(viewAdapter)
		{
			this.Title = "Test dialog";
			this.SetResultCommand = new RelayCommand(this.SetResult, this.CanSetResult);
		}

		public ICommand SetResultCommand { get; }

		private string _message;
		public string Message
		{
			get => this._message;
			set => SetProperty(ref this._message, value);
		}

		public string ResultMessage { get; private set; }

		private Task SetResult()
		{
			this.ResultMessage = this.Message;
			this.CloseCommand.Execute(null);
			return Task.CompletedTask;
		}
		private bool CanSetResult()
		{
			return !string.IsNullOrEmpty(this.Message);
		}

	}
}
