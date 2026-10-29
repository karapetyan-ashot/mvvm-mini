using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasySoftware.MvvmMini
{
	/// <summary>
	/// Base class for objects that notify bindings when their properties change.
	/// </summary>
	public abstract class BindableBase : INotifyPropertyChanged
	{
		/// <inheritdoc/>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises <see cref="PropertyChanged"/> for the given property.
		/// </summary>
		/// <param name="propName">The property name. When called from a property setter it is filled in automatically.</param>
		public void RaisePropertyChanged([CallerMemberName] string propName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}

		/// <summary>
		/// Sets a backing field and raises <see cref="PropertyChanged"/> if the value changed.
		/// </summary>
		/// <typeparam name="T">The property type.</typeparam>
		/// <param name="storage">The backing field.</param>
		/// <param name="value">The new value.</param>
		/// <param name="propName">The property name. When called from a property setter it is filled in automatically.</param>
		/// <returns><see langword="true"/> if the value changed; otherwise <see langword="false"/>.</returns>
		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propName = null)
		{
			if (!EqualityComparer<T>.Default.Equals(storage, value))
			{
				storage = value;
				RaisePropertyChanged(propName);
				return true;
			}

			return false;
		}
	}
}