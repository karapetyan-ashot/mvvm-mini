using System;
using System.Threading.Tasks;

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
		private IServiceProvider _serviceProvider;

		public ShellViewModel(IViewAdapter viewAdapter, IServiceProvider serviceProvider) : base(viewAdapter)
		{
			this._serviceProvider = serviceProvider;

			this.Title = "MvvmMini.Demo";
			this.InfoMessage = string.Empty;

			this.OpenWindowCommand = new RelayCommand(this.OpenWindow, this.CanOpenWindow);
			this.OpenWindowWithParamsCommand = new RelayCommand(this.OpenWindowWithParams);
			this.OpenDialogCommand = new RelayCommand(this.OpenDialog);
			this.OpenHandleWindowClosingCommand = new RelayCommand(OpenHandleWindowClosing);
			this.OpenHandleWindowLoadingCommand = new RelayCommand(OpenHandleWindowLoading);
		}

		public IRelayCommand OpenWindowCommand { get; }
		public IRelayCommand OpenWindowWithParamsCommand { get; }
		public IRelayCommand OpenDialogCommand { get; }
		public IRelayCommand OpenHandleWindowClosingCommand { get; }
		public IRelayCommand OpenHandleWindowLoadingCommand { get; }

		private string _infoMessage;
		public string InfoMessage
		{
			get => this._infoMessage;
			set => SetProperty(ref this._infoMessage, value);
		}

		private Task OpenWindow()
		{
			IDemoWindowViewModel demoWindow = this._serviceProvider.GetViewModel<IDemoWindowViewModel>();
			demoWindow.Show();
			return Task.CompletedTask;
		}
		private bool CanOpenWindow()
		{
			return true;
		}

		private Task OpenWindowWithParams()
		{
			IDemoWindowWithParamsViewModel demoWindow = this._serviceProvider.GetViewModel<IDemoWindowWithParamsViewModel>(DateTime.Now);
			demoWindow.Show();
			return Task.CompletedTask;
		}

		private Task OpenDialog()
		{
			IDemoDialogViewModel dialogViewModel = this._serviceProvider.GetViewModel<IDemoDialogViewModel>();
			dialogViewModel.ShowDialog();
			if (dialogViewModel.DialogResult != null)
			{
				this.InfoMessage += $"Dialog result is {dialogViewModel .DialogResult}{Environment.NewLine}";
			}
			else
			{
				this.InfoMessage += $"Dialog was canceled{Environment.NewLine}";
			}
			return Task.CompletedTask;
		}

		private Task OpenHandleWindowClosing()
		{
			IDemoHandleWindowClosingViewModel demoWindow = this._serviceProvider.GetViewModel<IDemoHandleWindowClosingViewModel>();
			demoWindow.Show();
			return Task.CompletedTask;
		}

		private Task OpenHandleWindowLoading()
		{
			IDemoHandleWindowLoadingViewModel demoWindow = this._serviceProvider.GetViewModel<IDemoHandleWindowLoadingViewModel>();
			demoWindow.Show();
			return Task.CompletedTask;
		}
	}
}