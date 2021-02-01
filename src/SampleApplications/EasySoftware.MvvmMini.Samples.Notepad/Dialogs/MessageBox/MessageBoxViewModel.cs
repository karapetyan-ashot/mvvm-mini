using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox
{
   public class MessageBoxViewModel : DialogViewModelBase, IMessageBoxDialog
   {
      public MessageBoxViewModel(IViewAdapter viewAdapter, string message, string title, MessageBoxButton buttons) : base(viewAdapter)
      {
         this.DialogResult = MessageBoxResult.Cancel;
         this.Title = title;
         this.Message = message;
         this.Buttons = buttons;
         this.SetResultCommand = new RelayCommand<MessageBoxResult>(this.SetResult);
      }

      public ICommand SetResultCommand { get; }

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

      public MessageBoxResult DialogResult { get; private set; }

      private Task SetResult(MessageBoxResult result)
      {
         this.DialogResult = result;
         this.CloseCommand.Execute(null);

         return Task.CompletedTask;
      }
   }
}
