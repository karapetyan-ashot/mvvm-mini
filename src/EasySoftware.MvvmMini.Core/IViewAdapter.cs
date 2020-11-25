using System;

namespace EasySoftware.MvvmMini.Core
{
	public interface IViewAdapter
	{
		event EventHandler Loaded;

		object DataContext { get; set; }
		object View { get; }

		void Show();
		void ShowDialog();
		void Close();
	}
}
