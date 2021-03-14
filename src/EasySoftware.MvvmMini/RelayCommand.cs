using System;
using System.Threading.Tasks;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
   public abstract class RelayCommandBase : IRelayCommand
   {
      public bool UseCommandManager { get; protected set; }
      public bool IsRunning { get; protected set; }

      private event EventHandler _canExecuteChanged;
      public event EventHandler CanExecuteChanged
      {
         add
         {
            if (this.UseCommandManager)
               CommandManager.RequerySuggested += value;
            else
               this._canExecuteChanged += value;
         }
         remove
         {
            if (this.UseCommandManager)
               CommandManager.RequerySuggested -= value;
            else
               this._canExecuteChanged -= value;
         }
      }

      public abstract void Execute(object parameter);

      public abstract bool CanExecute(object parameter);

      public void RaiseCanExecuteChanged()
      {
         if (this.UseCommandManager)
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
         this.UseCommandManager = useCommandManager;
      }

      public override async void Execute(object parameter)
      {
         this.IsRunning = true;
         await _execute((T)parameter);
         this.IsRunning = false;
         this.RaiseCanExecuteChanged();
      }

      public override bool CanExecute(object parameter)
      {
         if (this.IsRunning)
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
         this.UseCommandManager = useCommandManager;
      }

      public override async void Execute(object parameter)
      {
         this.IsRunning = true;
         await _execute();
         this.IsRunning = false;
         this.RaiseCanExecuteChanged();
      }

      public override bool CanExecute(object parameter)
      {
         if (this.IsRunning)
            return false;

         if (_canExecute == null)
            return true;

         return _canExecute();
      }
   }
}