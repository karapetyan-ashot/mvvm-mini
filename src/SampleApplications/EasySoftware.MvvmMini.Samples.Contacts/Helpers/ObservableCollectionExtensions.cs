using System.Collections.ObjectModel;

namespace EasySoftware.MvvmMini.Samples.Contacts.Helpers
{
	public static class ObservableCollectionExtensions
	{
		public static void Replace<T>(this ObservableCollection<T> collection, T oldItem, T newItem)
		{
			int index = collection.IndexOf(oldItem);
			if (index < 0)
			{
				collection.Add(newItem);
			}
			else
			{
				collection.Remove(oldItem);
				collection.Insert(index, newItem);
			}
		}
	}
}
