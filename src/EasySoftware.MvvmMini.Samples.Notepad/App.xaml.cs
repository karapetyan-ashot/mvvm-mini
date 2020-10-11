using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Notepad.Factories;

using Unity;
using Unity.Injection;

namespace EasySoftware.MvvmMini.Samples.Notepad
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		IUnityContainer _container;
		public App()
		{
			this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			ConfigureContainer();
			
			IWindowViewModel mainViewModel = this._container.Resolve<IWindowViewModel>(ViewModels.MainWindow);
			
			mainViewModel.Closed += MainViewModel_Closed;			
			mainViewModel.Show();
		}

		private void MainViewModel_Closed(object sender, EventArgs e)
		{
			this.Shutdown();
		}

		private void ConfigureContainer()
		{
			this._container = new UnityContainer().AddExtension(new Diagnostic());

			this._container.RegisterInstance<IUnityContainer>(this._container);

			this._container.RegisterSingleton<IDialogFactory, DialogFactory>();

			this._container.RegisterType<IView, ViewWrapper<MainView>>(ViewModels.MainWindow);
			this._container.RegisterType<IWindowViewModel, MainViewModel>(ViewModels.MainWindow,
				new InjectionConstructor(
					new ResolvedParameter<IView>(ViewModels.MainWindow),
					new ResolvedParameter<IDialogFactory>()));

			this._container.RegisterType<IView, ViewWrapper<MessageBoxView>>(ViewModels.MessageBoxViewModel);
			this._container.RegisterType<IMessageBoxDialog, MessageBoxViewModel>(ViewModels.MessageBoxViewModel,
				new InjectionConstructor(
					new ResolvedParameter<IView>(ViewModels.MessageBoxViewModel),
					new OptionalParameter<string>(),
					new OptionalParameter<string>(),
					new OptionalParameter<MessageBoxButton>()));

		}

	}
}
