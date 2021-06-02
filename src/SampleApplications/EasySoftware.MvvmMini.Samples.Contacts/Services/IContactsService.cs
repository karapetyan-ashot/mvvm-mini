using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoftware.MvvmMini.Samples.Contacts.Services
{
	public interface IContactsService
	{
		Task<UserModel> Login(string userName, string password);

		Task<List<ContactModel>> GetContacts();
		Task<ContactModel> GetContactById(int id);
		Task<ContactModel> CreateContact(ContactModel contact);
		Task<ContactModel> UpdateContact(ContactModel contact);
		Task DeleteContact(ContactModel contact);
	}
}