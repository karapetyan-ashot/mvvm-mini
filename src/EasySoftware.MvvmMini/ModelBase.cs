using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public abstract class ModelBase : IModel
	{
		protected readonly ErrorContainer _errorContainer = new ErrorContainer();

		public bool HasErrors => this._errorContainer.HasErrors;

		public IReadOnlyDictionary<string, IEnumerable<string>> Errors => this._errorContainer.Errors;

		public void AddError(string errorMessage)
		{
			this._errorContainer.AddError(errorMessage);
		}

		public void AddError(string propName, string errorMessage)
		{
			this._errorContainer.AddError(propName, errorMessage);
		}

		public void ClearErrors()
		{
			this._errorContainer.ClearErrors();
		}

		public void ClearErrors(string propName)
		{
			this._errorContainer.ClearErrors(propName);
		}
	}
}
