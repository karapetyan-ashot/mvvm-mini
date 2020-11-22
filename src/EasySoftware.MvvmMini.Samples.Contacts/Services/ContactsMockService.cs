using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.Win32.SafeHandles;

namespace EasySoftware.MvvmMini.Samples.Contacts.Services
{
	public class ContactsMockService : IContactsService
	{
		private static List<User> _users = new List<User> { new User { Id = 1, Name = "Name LastName", UserName = "username", Password = "1" } };
		private static List<Contact> _contacts;
		private static int _nextId = 0;
		public static int NextId { get { return ++_nextId; } }

		static ContactsMockService()
		{
			_contacts = new List<Contact>();

			for (int i = 1; i < 10; i++)
			{
				_contacts.Add(new Contact { Id = NextId, Name = $"Contact {i}", Email = $"email_{i}@gmail.com", Phone = $"33-44-{i}" });
			}
		}

		public async Task<User> Login(string userName, string password)
		{
			await Task.Delay(2000);
			return _users.SingleOrDefault(x => x.UserName.ToLower() == userName.ToLower() && x.Password == password);
		}

		public async Task<List<Contact>> GetContacts()
		{
			await Task.Delay(2000);
			return _contacts.Select(x => x.Clone()).ToList();
		}

		public async Task<Contact> GetContactById(int id)
		{
			await Task.Delay(2000);
			Contact found = _contacts.SingleOrDefault(x => x.Id == id);
			if (found != null)
				return found.Clone();
			return null;
		}

		public async Task<Contact> CreateContact(Contact contact)
		{
			await Task.Delay(2000);
			contact.Id = NextId;
			contact.Modified = DateTime.Now;
			_contacts.Add(contact.Clone());
			return contact;
		}

		public async Task<Contact> UpdateContact(Contact contact)
		{
			await Task.Delay(2000);
			Contact found = _contacts.SingleOrDefault(x => x.Id == contact.Id && x.Modified == contact.Modified);
			if (found == null)
				throw new DBConcurrencyException();
			contact.Modified = DateTime.Now;
			_contacts.Remove(found);
			_contacts.Add(contact);
			return contact;
		}

		public async Task DeleteContact(Contact contact)
		{
			await Task.Delay(2000);
			Contact found = _contacts.SingleOrDefault(x => x.Id == contact.Id && x.Modified == contact.Modified);
			if (found == null)
				throw new DBConcurrencyException();
			_contacts.Remove(found);
		}

	}
}
