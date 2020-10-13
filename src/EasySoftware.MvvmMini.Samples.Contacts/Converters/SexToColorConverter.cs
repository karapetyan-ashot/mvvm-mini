using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace EasySoftware.MvvmMini.Samples.Contacts.Converters
{
	public class SexToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Color color = Colors.Transparent;

			if(value is bool sex)
			{
				if (sex)
					color = Colors.LightBlue;
				else
					color = Colors.LightPink;
			}

			return new SolidColorBrush(color);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
