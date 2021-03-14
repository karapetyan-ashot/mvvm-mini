using System;
using System.Threading.Tasks;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
   public abstract class RelayCommandBase : IRelayCommand
   {
      protected bool _useCommandManager = true;
      protected bool _isRunning;

      private event EventHandler _canExecuteChanged;
      public event EventHandler CanExecuteChanged
      {
         add
         {
            if (this._useCommandManager)
               CommandManager.RequerySuggested += value;
            else
               this._canExecuteChanged += value;
         }
         remove
         {
            if (this._useCommandManager)
               CommandManager.RequerySuggested -= value;
            else
               this._canExecuteChanged -= value;
         }
      }

      public abstract void Execute(object parameter);

      public abstract bool CanExecute(object parameter);

      public void RaiseCanExecuteChanged()
      {
         if (this._useCommandManager)
            CommandManager.InvalidateRequerySuggested();
         else
            this._canExecuteChanged?.Invoke(this, EventArgs.Empty);
      }
   }

   public class RelayCommand<T> : RelayCommandBase
   {
      private readonly Func<T, Task> _execute;
      private readonly Func<T, bool> _canExecute;

      public RelayCommand(Func<T, Task> execute)
         : this(execute, null, false) { }

      public RelayCommand(Func<T, Task> execute, Func<T, bool> canExecute)
         : this(execute, canExecute, true) { }


      public RelayCommand(Func<T, Task> execute, Func<T, bool> canExecute, bool useCommandManager)
      {
         this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
         this._canExecute = canExecute;
         this._useCommandManager = useCommandManager;
      }

      public override async void Execute(object parameter)
      {
         this._isRunning = true;
         await _execute((T)parameter);
         this._isRunning = false;
         this.RaiseCanExecuteChanged();
      }

      public override bool CanExecute(object parameter)
      {
         if (this._isRunning)
            return false;

         if (_canExecute == null)
            return true;

         return _canExecute((T)parameter);
      }
   }

   public class RelayCommand : RelayCommandBase
   {
      private readonly Func<Task> _execute;
      private readonly Func<bool> _canExecute;

      public RelayCommand(Func<Task> execute)
         : this(execute, null, false) { }

      public RelayCommand(Func<Task> execute, Func<bool> canExecute)
         : this(execute, canExecute, true) { }

      public RelayCommand(Func<Task> execute, Func<bool> canExecute, bool useCommandManager)
      {
         this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
         this._canExecute = canExecute;
         this._useCommandManager = useCommandManager;
      }

      public override async void Execute(object parameter)
      {
         this._isRunning = true;
         await _execute();
         this._isRunning = false;
         this.RaiseCanExecuteChanged();
      }

      public override bool CanExecute(object parameter)
      {
         if (this._isRunning)
            return false;

         if (_canExecute == null)
            return true;

         return _canExecute();
      }
   }
}