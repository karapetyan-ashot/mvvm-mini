using System.ComponentModel;

using EasySoftware.Abstractions;

namespace EasySoftware.MvvmMini.Core
{
	/// <summary>
	/// A bindable model that reports property changes and validation errors.
	/// </summary>
	public interface IModel : INotifyPropertyChanged, INotifyDataErrorInfo, IErrorContainer { }
}