using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EasySoftware.MvvmMini.WpfDemo.Converters
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility result = Visibility.Hidden;

			if(value is bool val)
			{
				if (val) result = Visibility.Visible;
			}
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}