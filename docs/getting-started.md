# Getting Started

This guide builds a WPF application with a main window and a dialog. It takes about ten minutes.

## 1. Install the package

Create a WPF application for .NET 6 or later and add the **`EasySoftware.MvvmMini`** package:

```powershell
dotnet add package EasySoftware.MvvmMini
```

## 2. Create the shell view model

The shell is the main window. Define an interface for it and a class that derives from `WindowViewModelBase`:

```csharp
using System;
using System.Threading.Tasks;

using EasySoftware.MvvmMini;
using EasySoftware.MvvmMini.Core;

public interface IShellViewModel : IWindowViewModel { }

public class ShellViewModel : WindowViewModelBase, IShellViewModel
{
    private readonly IServiceProvider _serviceProvider;

    public ShellViewModel(IViewAdapter viewAdapter, IServiceProvider serviceProvider) : base(viewAdapter)
    {
        this._serviceProvider = serviceProvider;
        this.Title = "My application";
        this.AskNameCommand = new RelayCommand(this.AskName);
    }

    public IRelayCommand AskNameCommand { get; }

    private string _greeting;
    public string Greeting
    {
        get => this._greeting;
        set => SetProperty(ref this._greeting, value);
    }

    private Task AskName()
    {
        var dialog = this._serviceProvider.GetViewModel<INameDialogViewModel>();
        dialog.ShowDialog();
        if (dialog.DialogResult != null)
            this.Greeting = $"Hello, {dialog.DialogResult}!";
        return Task.CompletedTask;
    }
}
```

The `IViewAdapter` parameter is supplied by MvvmMini. Every other constructor parameter, such as `IServiceProvider` here, is resolved from the service container.

## 3. Create the shell view

The view is a normal WPF window. Bind to the view model's properties; you do not set the `DataContext` yourself.

```xml
<Window x:Class="MyApp.ShellView"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="{Binding Title}" Width="400" Height="200">
    <StackPanel Margin="10">
        <Button Command="{Binding AskNameCommand}" Content="Say hello" />
        <TextBlock Margin="0,10" Text="{Binding Greeting}" />
        <Button Command="{Binding CloseCommand}" Content="Close" />
    </StackPanel>
</Window>
```

## 4. Create a dialog

A dialog derives from `DialogViewModelBase<T>`, where `T` is the type of the result:

```csharp
public interface INameDialogViewModel : IDialogViewModel<string> { }

public class NameDialogViewModel : DialogViewModelBase<string>, INameDialogViewModel
{
    public NameDialogViewModel(IViewAdapter viewAdapter) : base(viewAdapter)
    {
        this.Title = "Your name";
        this.OkCommand = new RelayCommand(this.Ok);
    }

    public IRelayCommand OkCommand { get; }

    public string Name { get; set; }

    public override string DialogResult { get; protected set; }

    private Task Ok()
    {
        this.DialogResult = this.Name;
        this.CloseCommand.Execute(null);
        return Task.CompletedTask;
    }
}
```

Its view, `NameDialogView`, is a `Window` with a `TextBox` bound to `Name` and an OK button bound to `OkCommand`. If the user closes the dialog without pressing OK, `DialogResult` stays `null`.

## 5. Set up the application

In `App.xaml`, set `ShutdownMode` to `OnExplicitShutdown` and remove `StartupUri`, because MvvmMini opens the first window:

```xml
<Application x:Class="MyApp.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             ShutdownMode="OnExplicitShutdown" />
```

Then register your view models and show the shell in `App.xaml.cs`:

```csharp
using System.Windows;

using EasySoftware.MvvmMini;

using Microsoft.Extensions.DependencyInjection;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        IServiceCollection services = new ServiceCollection();

        // register your own services here, for example:
        // services.AddSingleton<IContactsService, ContactsService>();

        services.AddMvvmMini(mapper =>
        {
            mapper.RegisterViewModelWithView<IShellViewModel, ShellViewModel, ShellView>();
            mapper.RegisterViewModelWithView<INameDialogViewModel, NameDialogViewModel, NameDialogView>();
        });

        var serviceProvider = services.BuildServiceProvider();

        IShellViewModel shell = serviceProvider.GetViewModel<IShellViewModel>();
        shell.Closed += (s, args) => this.Shutdown();
        shell.Show();
    }
}
```

Run the application. The shell window opens, **Say hello** shows the dialog, and closing the shell ends the application.

## Next steps

- **Load data when the view appears:** override `Loaded()` in your view model and set `IsBusy` while it runs.
- **Ask before closing:** override `OnClosing` to ask "Do you want to save changes?" and cancel closing if needed. See [Closable view models](introduction.md#closable-view-models).
- **Pass parameters:** `GetViewModel<IEditorViewModel>(contact)` passes `contact` to the view model's constructor.
- **Validate input:** call `AddError` and `ClearErrors` in your property setters. See [Validation](introduction.md#validation).
- Explore the full [API Reference](api/EasySoftware.MvvmMini.yml) and the sample applications in the repository.
