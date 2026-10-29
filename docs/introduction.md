# Introduction

MvvmMini helps you build WPF applications with the Model-View-ViewModel pattern while keeping the amount of framework code small. This page explains the main ideas. To start coding, go to [Getting Started](getting-started.md).

## View model first

In MvvmMini, your code works with **view models**, not views. You define an interface for each view model, and register it together with its implementation and its view:

```csharp
services.AddMvvmMini(mapper =>
{
    mapper.RegisterViewModelWithView<IShellViewModel, ShellViewModel, ShellView>();
    mapper.RegisterViewModelWithView<IDocumentViewModel, DocumentViewModel, DocumentView>();
});
```

When you call `serviceProvider.GetViewModel<IDocumentViewModel>()`, MvvmMini:

1. creates the view (`DocumentView`), resolving its constructor dependencies from the service provider,
2. wraps it in an `IViewAdapter`,
3. creates the view model (`DocumentViewModel`), passing it the view adapter and resolving the rest of its constructor dependencies,
4. sets the view model as the view's `DataContext`.

The view model never references WPF types directly. It controls its view through `IViewAdapter`, which keeps it easy to unit test.

You can also pass values that are not registered in the container, such as the file to open:

```csharp
var document = serviceProvider.GetViewModel<IDocumentViewModel>(filePath);
```

## The view model hierarchy

| Interface | Base class | Use it for |
| --- | --- | --- |
| `IModel` | `ModelBase` | Data objects that notify property changes and hold validation errors. |
| `IViewModel` | `ViewModelBase` | Any view model with a view: a user control, a panel, a page. Adds `Title`, `IsBusy`, `View` and the overridable `Loaded` and `Unloaded` methods. |
| `IClosableViewModel` | `ClosableViewModelBase` | Anything with a lifecycle that ends when it is closed. |
| `IWindowViewModel` | `WindowViewModelBase` | A view model whose view is a window, shown with `Show()`. |
| `IDialogViewModel<T>` | `DialogViewModelBase<T>` | A modal dialog, shown with `ShowDialog()`, that returns a `DialogResult`. |

## Closable view models

A closable view model is not limited to windows and dialogs. It is anything the user opens, works with and then closes. In an IDE like Visual Studio, for example, an open file in the editor is closable, and so are tool panes such as Solution Explorer or Toolbox.

Closing always follows the same flow:

1. Something asks the view model to close: its `CloseCommand` (a Close button, a tab's close icon, a menu item) or, for a window, the user clicking X.
2. MvvmMini calls `OnClosing`. The view model can stop closing here by setting `e.Cancel = true`.
3. If closing goes ahead, the view is closed, `State` becomes `Closed` and the `Closed` event is raised, so the owner can remove the view model and release it.

A typical use of `OnClosing` is a document with unsaved changes:

```csharp
public override async Task OnClosing(CancelEventArgs e)
{
    if (!this.IsDirty)
        return;

    var answer = await this._dialogs.AskAsync("Do you want to save changes?");
    if (answer == Answer.Yes)
        await this.SaveAsync();
    else if (answer == Answer.Cancel)
        e.Cancel = true;
}
```

Override `CanClose` when closing should simply be unavailable for a while, for example during a save. Unlike `OnClosing`, it does not ask the user anything; it only enables or disables `CloseCommand`.

## Commands

`RelayCommand` and `RelayCommand<T>` wrap `async` methods:

```csharp
this.SaveCommand = new RelayCommand(this.SaveAsync, this.CanSave);
```

While the command runs, `IsRunning` is `true` and `CanExecute` returns `false`, so the same command cannot start twice. When you pass a `canExecute` function, WPF's `CommandManager` re-checks it automatically. Without one, call `RaiseCanExecuteChanged()` when the state changes.

## Validation

`ModelBase` (and so every view model) implements `INotifyDataErrorInfo` and `IErrorContainer`. Add errors with `AddError` and remove them with `ClearErrors`. WPF bindings show them automatically:

```csharp
this.ClearErrors(nameof(Name));
if (string.IsNullOrWhiteSpace(this.Name))
    this.AddError(nameof(Name), "Name is required.");
```

Errors that are not tied to a property are stored under an empty property name.

## Messaging between view models

`AddMvvmMini` registers a singleton `IEventAggregator`. View models can publish and subscribe to messages without referencing each other:

```csharp
eventAggregator.Subscribe<DocumentSaved>(this.OnDocumentSaved);
eventAggregator.Publish(new DocumentSaved(filePath));
```

Subscriptions are held through weak references, so they do not keep the subscriber alive. Subscribe with methods of the view model rather than with lambdas that capture local variables, because such lambdas can be garbage collected and stop receiving messages.

## WPF controls

The optional `EasySoftware.WpfControls` package contains `BusyIndicator`, a content control that covers its content with an overlay while a view model is busy:

```xml
<ctrl:BusyIndicator IsBusy="{Binding IsBusy}" Message="Loading...">
    <!-- your content -->
</ctrl:BusyIndicator>
```

It can also show progress, a progress message and a Cancel button.
