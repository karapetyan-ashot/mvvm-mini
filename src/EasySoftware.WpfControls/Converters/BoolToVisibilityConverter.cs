using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EasySoftware.WpfControls.Converters
{
    /// <summary>
    /// Converts <see langword="true"/> to <see cref="Visibility.Visible"/> and any other value to <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="bool"/> to a <see cref="Visibility"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used.</param>
        /// <returns><see cref="Visibility.Visible"/> if <paramref name="value"/> is <see langword="true"/>; otherwise <see cref="Visibility.Collapsed"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (value is bool b && b) ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// Converts a <see cref="Visibility"/> back to a <see cref="bool"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is <see cref="Visibility.Visible"/>; otherwise <see langword="false"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is Visibility v && v == Visibility.Visible;
    }
}
