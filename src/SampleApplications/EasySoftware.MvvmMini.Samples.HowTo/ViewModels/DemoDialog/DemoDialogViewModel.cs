using System.Threading.Tasks;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoDialog
{
	public interface IDemoDialogViewModel : IDialogViewModel<string>
	{		
	}

	public class DemoDialogViewModel : DialogViewModelBase<string>, IDemoDialogViewModel
	{
		public DemoDialogViewModel(IViewAdapter viewAdapter) : base(viewAdapter)
		{
			this.Title = "Test dialog";
			this.SetResultCommand = new RelayCommand(this.SetResult, this.CanSetResult);
		}

		public IRelayCommand SetResultCommand { get; }

		private string _message;
		public string Message
		{
			get => this._message;
			set => SetProperty(ref this._message, value);
		}

		
		public override string DialogResult { get; protected set; }

		private Task SetResult()
		{
			this.DialogResult = this.Message;
			this.CloseCommand.Execute(null);
			return Task.CompletedTask;
		}
		private bool CanSetResult()
		{
			return !string.IsNullOrEmpty(this.Message);
		}

	}
}
