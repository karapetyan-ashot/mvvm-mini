using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoftware.MvvmMini.Samples.Contacts.Services
{
   public class ContactsMockService : IContactsService
   {
      private static List<User> _users = new List<User> { new User { Id = 1, Name = "Ashot", UserName = "shota", Password = "1" } };
      
      public async Task<User> Login(string userName, string password)
      {
         await Task.Delay(2000);
         return _users.SingleOrDefault(x => x.UserName.ToLower() == userName.ToLower() && x.Password == password);
      }
   }
}
