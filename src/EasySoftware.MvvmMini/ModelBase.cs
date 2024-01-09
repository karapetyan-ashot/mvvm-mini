using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using EasySoftware.Abstractions;
using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
    public abstract partial class ModelBase : BindableBase, IModel { }

	public abstract partial class ModelBase : INotifyDataErrorInfo
	{
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		public IEnumerable GetErrors(string propertyName)
		{
			propertyName = propertyName ?? string.Empty;
			if (this.Errors.ContainsKey(propertyName))
				return this.Errors[propertyName];

			return null;
		}

		protected void RaiseErrorsChanged(string propName)
		{
			this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propName));
		}
	}

	public abstract partial class ModelBase : IErrorContainer
	{
		private readonly Dictionary<string, IEnumerable<string>> _errors;
		
        public ModelBase()
		{
			this._errors = new Dictionary<string, IEnumerable<string>>();			
		}

		public IReadOnlyDictionary<string, IEnumerable<string>> Errors
		{
			get => this._errors;
			set => this.CloneErrors(value);
		}

        public virtual bool HasErrors => this._errors.Any();

		public void AddError(string errorMessage)
		{
			this.AddError(string.Empty, errorMessage);
		}

		public void AddError(string propName, string errorMessage)
		{
			if (string.IsNullOrEmpty(errorMessage))
			{
				throw new ArgumentNullException(nameof(errorMessage));
			}

			propName = propName ?? string.Empty;

			if (!this._errors.ContainsKey(propName))
				this._errors.Add(propName, new List<string>());

			if (!this._errors[propName].Contains(errorMessage))
				((List<string>)this._errors[propName]).Add(errorMessage);

			this.RaiseErrorsChanged(propName);
		}

		public void ClearErrors()
		{
			foreach (string key in this._errors.Keys.ToList())
			{
				ClearErrors(key);
			}
		}

		public void ClearErrors(string propName)
		{
			propName = propName ?? string.Empty;

			if (this._errors.ContainsKey(propName))
			{
				this._errors.Remove(propName);
				this.RaiseErrorsChanged(propName);
			}
		}

		public virtual void CloneErrors(IErrorContainer other)
		{
			CloneErrors(other.Errors);
		}

		private void CloneErrors(IReadOnlyDictionary<string, IEnumerable<string>> otherErrors)
		{
			this.ClearErrors();

			if (otherErrors == null || !otherErrors.Any())
				return;

            foreach (var propError in otherErrors)
            {
                foreach (var error in propError.Value)
                {
                    this.AddError(propError.Key, error);
                }
            }
        }
	}
}