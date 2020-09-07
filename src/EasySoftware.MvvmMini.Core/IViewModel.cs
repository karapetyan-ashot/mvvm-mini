using System.Threading.Tasks;

namespace EasySoftware.MvvmMini.Core
{
	public interface IViewModel
	{
		string UniqueId { get; }
		IView View { get; }
		Task Loaded();
	}
}
