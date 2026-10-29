using System;

namespace EasySoftware.MvvmMini.Core
{
	/// <summary>
	/// Wraps a view so a view model can control it without depending on WPF types.
	/// </summary>
	public interface IViewAdapter
	{
		/// <summary>
		/// Occurs the first time the view is loaded.
		/// </summary>
		event EventHandler Loaded;

		/// <summary>
		/// Occurs when the view is unloaded after it was loaded.
		/// </summary>
		event EventHandler Unloaded;

		/// <summary>
		/// Gets or sets the data context of the view.
		/// </summary>
		object DataContext { get; set; }

		/// <summary>
		/// Gets the wrapped view.
		/// </summary>
		object View { get; }

		/// <summary>
		/// Shows the view as a non-modal window.
		/// </summary>
		void Show();

		/// <summary>
		/// Shows the view as a modal dialog and returns when it is closed.
		/// </summary>
		void ShowDialog();

		/// <summary>
		/// Closes the view.
		/// </summary>
		void Close();
	}
}
