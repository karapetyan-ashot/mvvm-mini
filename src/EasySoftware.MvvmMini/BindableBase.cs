using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasySoftware.MvvmMini
{
	public class BindableBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected void RaisePropertyChanged([CallerMemberName] string propName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}

		protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propName = null) where T : class
		{
			if (!EqualityComparer<T>.Default.Equals(storage, value))
			{
				storage = value;
				RaisePropertyChanged(propName);
			}
		}
	}
}
