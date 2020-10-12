using System.Threading.Tasks;

namespace EasySoftware.MvvmMini.Samples.Contacts.Services
{
   public interface IContactsService
   {
      Task<User> Login(string userName, string password);
   }
}
