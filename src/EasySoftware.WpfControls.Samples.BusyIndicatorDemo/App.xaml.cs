using System.Windows;

using EasySoftware.MvvmMini;
using EasySoftware.WpfControls.Samples.BusyIndicatorDemo.BLL;

using Microsoft.Extensions.DependencyInjection;

namespace EasySoftware.WpfControls.Samples.BusyIndicatorDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddScoped<IBllService, BllService>();

            services.AddMvvmMini(mapper => {
                mapper.RegisterViewModelWithView<IMainViewModel, MainViewModel, MainView>();
            });

            var sp = services.BuildServiceProvider();


            var mainViewModel = sp.GetViewModel<IMainViewModel>();
            mainViewModel.Closed += (s, ea) => this.Shutdown();
            mainViewModel.Show();
        }
    }

}
