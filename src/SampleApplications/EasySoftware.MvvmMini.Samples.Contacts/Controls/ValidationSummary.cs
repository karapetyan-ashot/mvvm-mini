using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace EasySoftware.MvvmMini.Samples.Contacts.Controls
{

	public partial class ValidationSummary : UserControl
	{
		private ListBox _errorsListBox = new ListBox();
		public ValidationSummary()
		{
			this.Content = this._errorsListBox;
			this.Loaded += ValidationSummary_Loaded; 
		}

		private void ValidationSummary_Loaded(object sender, RoutedEventArgs e)
		{
			if (this.DataContext is INotifyDataErrorInfo viewModel)
			{
				viewModel.ErrorsChanged += ViewModel_ErrorsChanged;
			}
			this.ShowErrors();
		}

		private void ViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
		{
			this.ShowErrors();
		}

		private void ShowErrors()
		{
			this.Visibility = Visibility.Hidden;
			this._errorsListBox.ItemsSource = null;

			if (this.DataContext is IErrorContainer viewModel)
			{
				var errors = viewModel.GetErrors().Select(x => $"{x.Key} - {x.Value}");
				this._errorsListBox.ItemsSource = errors;
				if (errors.Any())
					this.Visibility = Visibility.Visible;
			}

		}
	}

}
