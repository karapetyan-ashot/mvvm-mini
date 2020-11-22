using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoftware.MvvmMini.Samples.Contacts.Services
{
	public interface IContactsService
	{
		Task<User> Login(string userName, string password);

		Task<List<Contact>> GetContacts();
		Task<Contact> GetContactById(int id);
		Task<Contact> CreateContact(Contact contact);
		Task<Contact> UpdateContact(Contact contact);
		Task DeleteContact(Contact contact);
	}
}