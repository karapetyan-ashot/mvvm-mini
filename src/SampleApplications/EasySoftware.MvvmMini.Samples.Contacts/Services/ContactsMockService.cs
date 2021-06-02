using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoftware.MvvmMini.Samples.Contacts.Services
{
	public class ContactsMockService : IContactsService
	{
		private static List<UserModel> _users = new List<UserModel> { new UserModel { Id = 1, Name = "Name LastName", UserName = "username", Password = "1" } };
		private static List<ContactModel> _contacts;
		private static int _nextId = 0;
		public static int NextId { get { return ++_nextId; } }

		static ContactsMockService()
		{
			_contacts = new List<ContactModel>();

			for (int i = 1; i < 10; i++)
			{
				_contacts.Add(new ContactModel { Id = NextId, Name = $"Contact {i}", Email = $"email_{i}@gmail.com", Phone = $"33-44-{i}" });
			}
		}

		public async Task<UserModel> Login(string userName, string password)
		{
			await Task.Delay(2000);

			UserModel user = new UserModel { UserName = userName, Password = password };
			
			this.ValidateUser(user);

			if (user.HasErrors)
				return user;

			UserModel result = _users.SingleOrDefault(x => x.UserName.ToLower() == user.UserName.ToLower() && x.Password == user.Password);
			
			if(result == null)
			{
				user.AddError("Invalid username or password");
				return user;
			}

			return result;
		}

		public async Task<List<ContactModel>> GetContacts()
		{
			await Task.Delay(2000);
			
			return _contacts.Select(x => x.Clone()).ToList();
		}

		public async Task<ContactModel> GetContactById(int id)
		{
			await Task.Delay(2000);

			ContactModel found = _contacts.SingleOrDefault(x => x.Id == id);
			if (found != null)
				return found.Clone();
			return null;
		}

		public async Task<ContactModel> CreateContact(ContactModel contact)
		{
			await Task.Delay(2000);

			this.ValidateContact(contact);

			if (contact.HasErrors)
				return contact;

			contact.Id = NextId;
			contact.Modified = DateTime.Now;
			_contacts.Add(contact.Clone());
			return contact;
		}

		public async Task<ContactModel> UpdateContact(ContactModel contact)
		{
			await Task.Delay(2000);

			this.ValidateContact(contact);

			if (contact.HasErrors)
				return contact;

			ContactModel found = _contacts.SingleOrDefault(x => x.Id == contact.Id && x.Modified == contact.Modified);
			if (found == null)
				throw new DBConcurrencyException();
			contact.Modified = DateTime.Now;
			_contacts.Remove(found);
			_contacts.Add(contact);
			return contact;
		}

		public async Task DeleteContact(ContactModel contact)
		{
			await Task.Delay(2000);

			ContactModel found = _contacts.SingleOrDefault(x => x.Id == contact.Id && x.Modified == contact.Modified);
			if (found == null)
				throw new DBConcurrencyException();
			_contacts.Remove(found);
		}

		// server side full validation
		private void ValidateUser(UserModel user)
		{
			user.ClearErrors();

			if (string.IsNullOrEmpty(user.UserName))
				user.AddError(nameof(user.UserName), "username is required");
			if (user.UserName != null && user.UserName.Length < 2)
				user.AddError(nameof(user.UserName), "username lenght must be >= 2");
			if (string.IsNullOrEmpty(user.Password))
				user.AddError(nameof(user.Password), "password is required");
		}

		// server side full validation
		private void ValidateContact(ContactModel contact)
		{
			contact.ClearErrors();
			if (string.IsNullOrEmpty(contact.Name))
				contact.AddError(nameof(contact.Name), "Name is required");
			if (string.IsNullOrEmpty(contact.Phone))
				contact.AddError(nameof(contact.Phone), "Phone is required");
			if (string.IsNullOrEmpty(contact.Email))
				contact.AddError(nameof(contact.Email), "Email is required");
			else
			{
				if (!contact.Email.Contains("@"))
					contact.AddError(nameof(contact.Email), "Not valid email");
			}
		}
	}
}