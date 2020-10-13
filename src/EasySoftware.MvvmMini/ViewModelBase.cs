using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public partial class ViewModelBase : BindableBase, IViewModel
	{
		protected IView _view;

		public ViewModelBase(IView view)
		{
			this._view = view;
			this._view.DataContext = this;
			this._view.Loaded += async (s, e) => await this.Loaded();
		}

		private string _title;
		public string Title
		{
			get { return this._title; }
			set
			{
				if (this._title != value)
				{
					this._title = value;
					this.RaisePropertyChanged();
				}
			}
		}

		private bool _isBusy;
		public bool IsBusy
		{
			get => this._isBusy;
			set
			{
				if (this._isBusy != value)
				{
					this._isBusy = value;
					RaisePropertyChanged();
				}
			}
		}

		public object View => this._view.View;

		protected virtual Task Loaded() { return Task.CompletedTask; }
	}

	public partial class ViewModelBase : IDataErrorInfo
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

	public partial class ViewModelBase : INotifyDataErrorInfo
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

	public partial class ViewModelBase : IErrorContainer
	{
		public void ClearErrors()
		{
			if (this._errors != null)
			{
				this._errors.Clear();
				this.RaiseErrorsChanged(string.Empty);
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
			this.RemoveError(string.Empty, propName);
		}

		public void RemoveError(string propName, string errorMessage)
		{
			if(this._errors != null && this._errors.ContainsKey(propName))
			{
				if(this._errors[propName].Contains(errorMessage))
				{
					this._errors[propName].Remove(errorMessage);
					this.RaiseErrorsChanged(propName);
				}
			}
		}
	}
}
