using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySoftware.MvvmMini.Core
{
	/// <summary>
	/// A view model with a lifecycle that ends when it is closed.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A closable view model is not limited to windows and dialogs. It can be anything the user opens,
	/// works with and then closes, for example a document tab in an editor, a panel or a wizard page.
	/// </para>
	/// <para>
	/// Before closing, the view model gets a chance to stop it in <see cref="OnClosing(CancelEventArgs)"/>.
	/// A typical use is a document with unsaved changes: it asks the user "Do you want to save changes?",
	/// saves on Yes, closes on No, and cancels closing on Cancel.
	/// </para>
	/// <para>
	/// When closing goes ahead, <see cref="Closed"/> is raised so the owner, such as a shell that keeps
	/// the list of open documents, can remove the view model and release it.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	/// public override async Task OnClosing(CancelEventArgs e)
	/// {
	///     if (!this.IsDirty)
	///         return;
	///
	///     var answer = await this._dialogs.AskAsync("Do you want to save changes?");
	///     if (answer == Answer.Yes)
	///         await this.SaveAsync();
	///     else if (answer == Answer.Cancel)
	///         e.Cancel = true;
	/// }
	/// </code>
	/// </example>
	public interface IClosableViewModel : IViewModel
	{
		/// <summary>
		/// Occurs after the view model has been closed. The owner of the view model, for example
		/// the shell that hosts it as a document, can handle it to remove and release the view model.
		/// </summary>
		event EventHandler Closed;

		/// <summary>
		/// Gets the command that asks the view model to close. Bind it to a Close button, a tab's
		/// close icon or a menu item. Closing still goes through <see cref="OnClosing(CancelEventArgs)"/>
		/// and can be cancelled there.
		/// </summary>
		ICommand CloseCommand { get; }

		/// <summary>
		/// Called before the view model is closed, so it can decide whether closing may go ahead.
		/// For example, a view model with unsaved changes can ask the user whether to save them,
		/// and set <see cref="CancelEventArgs.Cancel"/> to <see langword="true"/> to stay open
		/// if the user cancels.
		/// </summary>
		/// <param name="e">The event data. Set <see cref="CancelEventArgs.Cancel"/> to <see langword="true"/> to cancel closing.</param>
		/// <returns>A task that completes when the view model has decided whether to close.</returns>
		Task OnClosing(CancelEventArgs e);
	}
}
