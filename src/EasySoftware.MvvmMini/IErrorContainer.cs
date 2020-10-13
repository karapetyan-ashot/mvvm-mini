using System;

namespace EasySoftware.MvvmMini
{
	public interface IErrorContainer
	{
		bool HasErrors { get; }
		void ClearErrors();		

		void AddError(string errorMessage);
		void AddError(string propName, string errorMessage);
		void AddError(Exception ex);

		void RemoveError(string propName);
		void RemoveError(string propName, string errorMessage);
	}
}
