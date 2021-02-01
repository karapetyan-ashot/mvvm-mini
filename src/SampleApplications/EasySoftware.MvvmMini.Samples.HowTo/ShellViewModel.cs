using System;
using System.Threading.Tasks;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoDialog;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoHandleWindowClosing;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoHandleWindowLoading;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoWindow;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoWindowWithParams;

namespace EasySoftware.MvvmMini.Samples.HowTo
{
	public interface IShellViewModel : IWindowViewModel { }

	public class ShellViewModel : WindowViewModelBase, IShellViewModel
	{
		private IViewModelFactory _viewModelFactory;

		public ShellViewModel(IViewAdapter viewAdapter, IViewModelFactory viewModelFactory) : base(viewAdapter)
		{
			this._viewModelFactory = viewModelFactory;

			this.Title = "MvvmMini.Demo";
			this.InfoMessage = string.Empty;

			this.OpenWindowCommand = new RelayCommand(this.OpenWindow, this.CanOpenWindow);
			this.OpenWindowWithParamsCommand = new RelayCommand(this.OpenWindowWithParams);
			this.OpenDialogCommand = new RelayCommand(this.OpenDialog);
			this.OpenHandleWindowClosingCommand = new RelayCommand(OpenHandleWindowClosing);
			this.OpenHandleWindowLoadingCommand = new RelayCommand(OpenHandleWindowLoading);
		}

		public ICommand OpenWindowCommand { get; }
		public ICommand OpenWindowWithParamsCommand { get; }
		public ICommand OpenDialogCommand { get; }
		public ICommand OpenHandleWindowClosingCommand { get; }
		public ICommand OpenHandleWindowLoadingCommand { get; }

		private string _infoMessage;
		public string InfoMessage
		{
			get => this._infoMessage;
			set => SetProperty(ref this._infoMessage, value);
		}



		private Task OpenWindow()
		{
			IDemoWindowViewModel demoWindow = this._viewModelFactory.ResolveViewModel<IDemoWindowViewModel>();
			demoWindow.Show();
			return Task.CompletedTask;
		}
		private bool CanOpenWindow()
		{
			return true;
		}

		private Task OpenWindowWithParams()
		{
			IDemoWindowWithParamsViewModel demoWindow = this._viewModelFactory.ResolveViewModel<IDemoWindowWithParamsViewModel>(("dateTime", DateTime.Now));
			demoWindow.Show();
			return Task.CompletedTask;
		}

		private Task OpenDialog()
		{
			IDemoDialogViewModel dialogViewModel = this._viewModelFactory.ResolveViewModel<IDemoDialogViewModel>();
			dialogViewModel.ShowDialog();
			if (dialogViewModel.ResultMessage != null)
			{
				this.InfoMessage += $"Dialog result is {dialogViewModel.ResultMessage}{Environment.NewLine}";
			}
			else
			{
				this.InfoMessage += $"Dialog was canceled{Environment.NewLine}";
			}
			return Task.CompletedTask;
		}

		private Task OpenHandleWindowClosing()
		{
			IDemoHandleWindowClosingViewModel demoWindow = this._viewModelFactory.ResolveViewModel<IDemoHandleWindowClosingViewModel>();
			demoWindow.Show();
			return Task.CompletedTask;
		}

		private Task OpenHandleWindowLoading()
		{
			IDemoHandleWindowLoadingViewModel demoWindow = this._viewModelFactory.ResolveViewModel<IDemoHandleWindowLoadingViewModel>();
			demoWindow.Show();
			return Task.CompletedTask;
		}



	}
}