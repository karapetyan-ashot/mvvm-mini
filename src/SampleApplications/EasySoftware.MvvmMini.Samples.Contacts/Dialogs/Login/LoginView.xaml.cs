using System.Windows;

namespace EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login
{
	/// <summary>
	/// Interaction logic for LoginView.xaml
	/// </summary>
	public partial class LoginView : Window
	{
		public LoginView()
		{
			InitializeComponent();
			this._loginTextBox.Focus();
		}
	}
}