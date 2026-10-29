---
_layout: landing
---

# MvvmMini

MvvmMini is a small MVVM library for WPF that uses a **view-model-first** approach. You ask for a view model by its interface, and MvvmMini creates the view model together with its view, binds them, and gives you simple methods to show, close and track it.

It is built on top of `Microsoft.Extensions.DependencyInjection`, so your view models and views get their dependencies through constructor injection.

## Install

The NuGet package ID is **`EasySoftware.MvvmMini`**:

```powershell
dotnet add package EasySoftware.MvvmMini
```

It brings in `EasySoftware.MvvmMini.Core` and `EasySoftware.Abstractions` automatically. MvvmMini supports .NET 6 to .NET 10 on Windows.

| Package | What it contains |
| --- | --- |
| `EasySoftware.MvvmMini` | Base classes (`ViewModelBase`, `WindowViewModelBase`, `DialogViewModelBase`, ...), `RelayCommand`, `EventAggregator` and the DI setup (`AddMvvmMini`, `GetViewModel`). |
| `EasySoftware.MvvmMini.Core` | The interfaces (`IViewModel`, `IClosableViewModel`, `IWindowViewModel`, `IDialogViewModel`, ...). Reference it from projects that should not depend on the implementation. |
| `EasySoftware.Abstractions` | `IErrorContainer`, a UI-independent way to store validation errors. |
| `EasySoftware.WpfControls` | Optional WPF controls, such as `BusyIndicator`. |

## What you get

- **View-model-first navigation.** Register each view model interface with its view model and view once, then call `serviceProvider.GetViewModel<IMyViewModel>()` anywhere.
- **Lifecycle management.** Windows, dialogs, document tabs and tool panes all close through one flow, with a chance to cancel, for example to ask "Do you want to save changes?".
- **Async commands.** `RelayCommand` runs `async` work and disables itself while it is running.
- **Validation.** Every model implements `INotifyDataErrorInfo`, so WPF shows your errors without extra code.
- **Messaging.** `EventAggregator` lets view models talk to each other without holding references.

## Next steps

- Read the [Introduction](introduction.md) for the main concepts.
- Follow [Getting Started](getting-started.md) to build your first window.
- Browse the [API Reference](api/EasySoftware.MvvmMini.yml).
- See the sample applications on [GitHub](https://github.com/karapetyan-ashot/mvvm-mini).
