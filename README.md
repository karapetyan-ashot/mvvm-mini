# mvvm-mini

<!-- Headings -->

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

		IViewModelFactory viewModelFactory = new ViewModelFactory();

		// register in viewmodel factory shell viewmodel and view
		viewModelFactory.RegisterViewModelWithView<IShellViewModel, ShellViewModel, ShellView>();

		// resolve shell viewmodel
		IShellViewModel shellViewModel = viewModelFactory.ResolveViewModel<IShellViewModel>();

		// subscribe to closed event to shutdown app
		shellViewModel.Closed += (s, ea) => this.Shutdown();

		// call show method to display the view.
		shellViewModel.Show();
	}
}
```

