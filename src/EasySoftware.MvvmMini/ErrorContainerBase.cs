﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EasySoftware.MvvmMini
{
	public abstract partial class ErrorContainerBase : BindableBase { }

	// IDataErrorInfo
	public partial class ErrorContainerBase : IDataErrorInfo
	{
		private Dictionary<string, List<string>> _errors;

		public string Error
		{
			get
			{
				string err = string.Empty;
				if (this._errors != null)
				{
					if (this._errors.ContainsKey(string.Empty))
						err += this[string.Empty];
					foreach (KeyValuePair<string, List<string>> item in this._errors.Where(x => x.Key != string.Empty))
					{
						err += this[item.Key];
					}
				}
				return err;
			}
		}

		public string this[string columnName]
		{
			get
			{
				string err = string.Empty;
				if (this._errors != null && this._errors.ContainsKey(columnName))
				{
					err += columnName + System.Environment.NewLine;
					foreach (string errMsg in this._errors[columnName])
					{
						err += string.Format("\t - {0}{1}", columnName, errMsg, System.Environment.NewLine);
					}
				}
				return err;
			}
		}
	}

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
			if (this._errors != null && this._errors.ContainsKey(propertyName))
				return this._errors[propertyName];

			return null;
		}

		public bool HasErrors
		{
			get { return this._errors == null ? false : this._errors.Any(); }
		}
	}

	// IErrorContainer
	public partial class ErrorContainerBase : IErrorContainer
	{
		public void ClearErrors()
		{
			if (this._errors != null)
			{
				foreach (var item in this._errors.ToList())
				{
					this.RemoveError(item.Key);
				}
			}
		}

		public void AddError(string errorMessage)
		{
			this.AddError(string.Empty, errorMessage);
		}

		public void AddError(Exception ex)
		{
			this.AddError(string.Empty, ex.Message);
		}

		public void AddError(string propName, string errorMessage)
		{
			if (this._errors == null)
				this._errors = new Dictionary<string, List<string>>();
			if (!this._errors.ContainsKey(propName))
				this._errors.Add(propName, new List<string>());
			if (!this._errors[propName].Contains(errorMessage))
				this._errors[propName].Add(errorMessage);
			this.RaiseErrorsChanged(propName);
		}

		public void RemoveError(string propName)
		{
			this.RemoveError(propName, string.Empty);
		}

		public void RemoveError(string propName, string errorMessage)
		{
			if (this._errors != null && this._errors.ContainsKey(propName))
			{
				if (string.IsNullOrEmpty(errorMessage))
				{
					this._errors.Remove(propName);
				}
				else if (this._errors[propName].Contains(errorMessage))
				{
					this._errors[propName].Remove(errorMessage);
					if (this._errors[propName].Count == 0)
						this._errors.Remove(propName);
				}

				this.RaiseErrorsChanged(propName);
			}
		}

		public Dictionary<string, string> GetErrors()
		{
			Dictionary<string, string> result = new Dictionary<string, string>();

			if (this._errors != null)
			{
				string err;
				foreach (KeyValuePair<string, List<string>> item in this._errors)
				{
					err = string.Empty;
					foreach (string errMsg in item.Value)
					{
						err += $"{errMsg}{Environment.NewLine}";
					}
					result.Add(item.Key, err);
				}
			}

			return result;
		}
	}
}