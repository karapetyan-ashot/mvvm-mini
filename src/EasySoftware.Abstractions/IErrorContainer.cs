using System.Collections.Generic;

namespace EasySoftware.Abstractions
{
    public interface IErrorContainer
	{
		bool HasErrors { get; }
		IReadOnlyDictionary<string, IEnumerable<string>> Errors { get; set; }
		void AddError(string errorMessage);
		void AddError(string propName, string errorMessage);
		void ClearErrors();
		void ClearErrors(string propName);
		void CloneErrors(IErrorContainer other);
	}
}