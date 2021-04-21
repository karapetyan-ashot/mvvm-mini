using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySoftware.MvvmMini.Core
{
	public interface IErrorContainer
	{
		bool HasErrors { get; }
		IReadOnlyDictionary<string, IEnumerable<string>> Errors { get; }
		void AddError(string errorMessage);
		void AddError(string propName, string errorMessage);
		void ClearErrors();
		void ClearErrors(string propName);
	}
}
