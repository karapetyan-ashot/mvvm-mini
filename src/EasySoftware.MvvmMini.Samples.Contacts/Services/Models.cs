using System;

namespace EasySoftware.MvvmMini.Samples.Contacts.Services
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
	}

	public class Contact
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool Sex { get; set; }
		public string Email { get; set; }
		public DateTime Modified { get; set; }

		public Contact Clone()
		{
			Contact clone = new Contact
			{
				Id = this.Id,
				Name = this.Name,
				Sex = this.Sex,
				Email = this.Email,
				Modified = this.Modified,
			};

			return clone;
		}
	}
}
