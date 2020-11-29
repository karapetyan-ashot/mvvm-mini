using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySoftware.MvvmMini
{
   public class RelayCommand<T> : ICommand
   {
      private readonly Func<T, Task> _execute;
      private readonly Func<T, bool> _canExecute;
      private bool _isLoad;

      public RelayCommand(Func<T, Task> execute, Func<T, bool> canExecute = null)
      {
         _execute = execute;
         _canExecute = canExecute;
      }

      public bool CanExecute(object parameter)
      {
         if (this._isLoad)
            return false;

         if (_canExecute != null)
            return _canExecute((T)parameter);

         return true;
      }

      public async void Execute(object parameter)
      {
         this._isLoad = true;
         await _execute((T)parameter);
         this._isLoad = false;
         this.RaiseCanExecuteChanged();
      }

      public event EventHandler CanExecuteChanged
      {
         add
         {
            CommandManager.RequerySuggested += value;
            LocalCanExecuteChanged += value;
         }
         remove
         {
            CommandManager.RequerySuggested -= value;
            LocalCanExecuteChanged -= value;
         }
      }

      public event EventHandler LocalCanExecuteChanged;

      public void RaiseCanExecuteChanged()
      {
         LocalCanExecuteChanged?.Invoke(this, new EventArgs());
      }
   }

   public class RelayCommand : ICommand
   {
      private readonly Func<Task> _execute;
      private readonly Func<bool> _canExecute;
      private bool _isLoad;

      public RelayCommand(Func<Task> execute, Func<bool> canExecute = null)
      {
         _execute = execute;
         _canExecute = canExecute;
      }

      public bool CanExecute(object parameter)
      {
         if (this._isLoad)
            return false;

         if (_canExecute != null)
            return _canExecute();

         return true;
      }

      public async void Execute(object parameter)
      {
         this._isLoad = true;
         await _execute();
         this._isLoad = false;
         this.RaiseCanExecuteChanged();
      }

      public event EventHandler CanExecuteChanged
      {
         add
         {
            CommandManager.RequerySuggested += value;
            LocalCanExecuteChanged += value;
         }
         remove
         {
            CommandManager.RequerySuggested -= value;
            LocalCanExecuteChanged -= value;
         }
      }

      public event EventHandler LocalCanExecuteChanged;

      public void RaiseCanExecuteChanged()
      {
         LocalCanExecuteChanged?.Invoke(this, new EventArgs());
      }
   }
}
