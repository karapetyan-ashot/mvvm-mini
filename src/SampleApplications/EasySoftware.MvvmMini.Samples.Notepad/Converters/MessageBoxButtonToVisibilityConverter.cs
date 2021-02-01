using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EasySoftware.MvvmMini.Samples.Notepad.Converters
{
	public class MessageBoxButtonToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = Visibility.Collapsed;
			if (value is MessageBoxButton button)
			{
				if (parameter is MessageBoxResult result)
				{
					switch (result)
					{
						case MessageBoxResult.OK:
							if (button == MessageBoxButton.OK || button == MessageBoxButton.OKCancel)
								visibility = Visibility.Visible;
							break;
						case MessageBoxResult.Cancel:
							if (button == MessageBoxButton.YesNoCancel || button == MessageBoxButton.OKCancel)
								visibility = Visibility.Visible;
							break;
						case MessageBoxResult.Yes:
							if (button == MessageBoxButton.YesNoCancel || button == MessageBoxButton.YesNo)
								visibility = Visibility.Visible;
							break;
						case MessageBoxResult.No:
							if (button == MessageBoxButton.YesNo || button == MessageBoxButton.YesNoCancel)
								visibility = Visibility.Visible;
							break;
					}
				}
			}
			return visibility;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
