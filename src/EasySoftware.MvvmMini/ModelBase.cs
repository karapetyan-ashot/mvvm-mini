using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using EasySoftware.Abstractions;
using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
    /// <summary>
    /// Base class for bindable models with built-in validation error storage.
    /// Implements <see cref="INotifyPropertyChanged"/>, <see cref="INotifyDataErrorInfo"/> and <see cref="IErrorContainer"/>.
    /// </summary>
    /// <remarks>
    /// Object-level errors are stored under the <see cref="string.Empty"/> key.
    /// </remarks>
    public abstract partial class ModelBase : BindableBase, IModel { }

	public abstract partial class ModelBase : INotifyDataErrorInfo
	{
		/// <inheritdoc/>
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		/// <summary>
		/// Gets the errors of the specified property, or the object-level errors when <paramref name="propertyName"/> is <see langword="null"/> or empty.
		/// </summary>
		/// <param name="propertyName">The property name, or <see langword="null"/> or empty for object-level errors.</param>
		/// <returns>The errors of the property, or <see langword="null"/> if it has none.</returns>
		public virtual IEnumerable GetErrors(string propertyName)
		{
			propertyName = propertyName ?? string.Empty;
			if (this.Errors.ContainsKey(propertyName))
				return this.Errors[propertyName];

			return null;
		}

		/// <summary>
		/// Raises <see cref="ErrorsChanged"/> for the specified property.
		/// </summary>
		/// <param name="propName">The property name, or <see cref="string.Empty"/> for object-level errors.</param>
		public void RaiseErrorsChanged(string propName)
		{
			this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propName));
		}
	}

	public abstract partial class ModelBase : IErrorContainer
	{
		private readonly Dictionary<string, IEnumerable<string>> _errors;
		
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase"/> class with no errors.
        /// </summary>
        public ModelBase()
		{
			this._errors = new Dictionary<string, IEnumerable<string>>();			
		}

		/// <inheritdoc/>
		public IReadOnlyDictionary<string, IEnumerable<string>> Errors
		{
			get => this._errors;
			set => this.CloneErrors(value);
		}

        /// <inheritdoc cref="IErrorContainer.HasErrors"/>
        public virtual bool HasErrors => this._errors.Any();

		/// <inheritdoc/>
		/// <exception cref="ArgumentNullException"><paramref name="errorMessage"/> is <see langword="null"/> or empty.</exception>
		public void AddError(string errorMessage)
		{
			this.AddError(string.Empty, errorMessage);
		}

		/// <summary>
		/// Adds an error for the specified property and raises <see cref="ErrorsChanged"/>. A message that the property already has is not added twice.
		/// </summary>
		/// <param name="propName">The property name, or <see langword="null"/> or empty for an object-level error.</param>
		/// <param name="errorMessage">The error message to add.</param>
		/// <exception cref="ArgumentNullException"><paramref name="errorMessage"/> is <see langword="null"/> or empty.</exception>
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

		/// <summary>
		/// Removes all errors and raises <see cref="ErrorsChanged"/> for each property that had errors.
		/// </summary>
		public void ClearErrors()
		{
			foreach (string key in this._errors.Keys.ToList())
			{
				ClearErrors(key);
			}
		}

		/// <summary>
		/// Removes all errors of the specified property and raises <see cref="ErrorsChanged"/> if it had any.
		/// </summary>
		/// <param name="propName">The property name, or <see langword="null"/> or empty for object-level errors.</param>
		public void ClearErrors(string propName)
		{
			propName = propName ?? string.Empty;

			if (this._errors.ContainsKey(propName))
			{
				this._errors.Remove(propName);
				this.RaiseErrorsChanged(propName);
			}
		}

		/// <inheritdoc/>
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