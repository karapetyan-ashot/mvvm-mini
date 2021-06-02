using System.Threading.Tasks;
using System.Windows;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox
{
	public class MessageBoxViewModel : DialogViewModelBase<MessageBoxResult>, IMessageBoxViewModel
	{
		public MessageBoxViewModel(IViewAdapter viewAdapter, string message, string title, MessageBoxButton buttons) : base(viewAdapter)
		{
			this.DialogResult = MessageBoxResult.Cancel;
			this.Title = title;
			this.Message = message;
			this.Buttons = buttons;
			this.SetResultCommand = new RelayCommand<MessageBoxResult>(this.SetResult);
		}

		public IRelayCommand SetResultCommand { get; }

		public override MessageBoxResult DialogResult { get; protected set; }

		private string _message;
		public string Message
		{
			get => this._message;
			set => SetProperty(ref this._message, value);
		}

		private MessageBoxButton _buttons;
		public MessageBoxButton Buttons
		{
			get => this._buttons;
			set => SetProperty(ref this._buttons, value);
		}

		private Task SetResult(MessageBoxResult result)
		{
			this.DialogResult = result;
			this.CloseCommand.Execute(null);

			return Task.CompletedTask;
		}
	}
}