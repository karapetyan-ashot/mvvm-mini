using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public abstract partial class ErrorContainerBase : BindableBase { }

	// INotifyDataErrorInfo
	public partial class ErrorContainerBase : INotifyDataErrorInfo
	{
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
		protected void RaiseErrorsChanged(string propName)
		{
			this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propName));
		}

		public System.Collections.IEnumerable GetErrors(string propertyName)
		{
			if (this._errorContainer.Errors.ContainsKey(propertyName))
				return this._errorContainer.Errors[propertyName];

			return null;
		}

		public bool HasErrors => this._errorContainer.HasErrors;
	}

	// IErrorContainer
	public partial class ErrorContainerBase : IErrorContainer
	{
		protected readonly ErrorContainer _errorContainer = new ErrorContainer();

		bool IErrorContainer.HasErrors => this._errorContainer.HasErrors;

		public IReadOnlyDictionary<string, IEnumerable<string>> Errors => this._errorContainer.Errors;

		public void AddError(string errorMessage)
		{
			this._errorContainer.AddError(errorMessage);
			RaiseErrorsChanged(string.Empty);
		}

		public void AddError(string propName, string errorMessage)
		{
			this._errorContainer.AddError(propName, errorMessage);
			RaiseErrorsChanged(propName);
		}

		public void ClearErrors()
		{
			foreach (string key in this._errorContainer.Errors.Keys.ToList())
			{
				ClearErrors(key);
			}
		}

		public void ClearErrors(string propName)
		{
			this._errorContainer.ClearErrors(propName);
			RaiseErrorsChanged(propName);
		}
	}
}
