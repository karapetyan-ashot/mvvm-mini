using System;
using System.Threading.Tasks;

namespace EasySoftware.MvvmMini.WpfDemo.Services
{
	public interface IDateTimeService
	{
		Task<DateTime> GetDate();
	}

	public class DateTimeService : IDateTimeService
	{
		public async Task<DateTime> GetDate()
		{
			await Task.Delay(1500);
			return DateTime.Now;
		}
	}
}