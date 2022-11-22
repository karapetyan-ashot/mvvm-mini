# mvvm-mini

## Quick start

### Create viewmodel with view for main window

```csharp
using EasySoftware.MvvmMini.Core;
...
public interface IShellViewModel : IWindowViewModel { }

public class ShellViewModel : WindowViewModelBase, IShellViewModel
{
	public ShellViewModel(IViewAdapter viewAdapter) : base(viewAdapter)
	{
		this.Title = "Shell window title";
	}
}
```

### Set ShutdownMode in App.xaml
```xaml
<Application 
...
ShutdownMode="OnExplicitShutdown" />

```

### Override OnStartup method of your App class
```csharp
using EasySoftware.MvvmMini.Core;
...
public partial class App : Application
{
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		// create services
		IServiceCollection services = new ServiceCollection();

		// setup MvvmMini
		services.AddMvvmMini(mapper => {
                mapper.RegisterViewModelWithView<IShellViewModel, ShellViewModel, ShellView>();
				// register other view models with views here
            });

		// create serviceProvider
		var sp = services.BuildServiceProvider();
		// resolve shell viewmodel
		IShellViewModel shellViewModel = sp.GetViewModel<IShellViewModel>();

		// subscribe to closed event to shutdown app
		shellViewModel.Closed += (s, ea) => this.Shutdown();

		// call show method to display the view.
		shellViewModel.Show();
	}
}
```

