using System;
using System.Collections.Generic;

namespace EasySoftware.MvvmMini
{
	public interface IErrorContainer
	{
		bool HasErrors { get; }
		void ClearErrors();

		Dictionary<string, string> GetErrors();

		void AddError(string errorMessage);
		void AddError(string propName, string errorMessage);
		void AddError(Exception ex);

		void RemoveError(string propName);
		void RemoveError(string propName, string errorMessage);
	}
}
