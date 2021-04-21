using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public class ErrorContainer : IErrorContainer
	{
		private readonly Dictionary<string, IEnumerable<string>> _errors = new Dictionary<string, IEnumerable<string>>();

		public ErrorContainer()
		{
			this.Errors = new ReadOnlyDictionary<string, IEnumerable<string>>(this._errors);
		}

		public bool HasErrors => this._errors.Any();

		public IReadOnlyDictionary<string, IEnumerable<string>> Errors { get; }

		public void AddError(string errorMessage)
		{
			AddError(string.Empty, errorMessage);
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
				this._errors.Remove(propName);
		}
	}
}