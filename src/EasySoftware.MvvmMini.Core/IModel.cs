using System.ComponentModel;

using EasySoftware.Abstractions;

namespace EasySoftware.MvvmMini.Core
{
	public interface IModel : INotifyPropertyChanged, INotifyDataErrorInfo, IErrorContainer { }
}