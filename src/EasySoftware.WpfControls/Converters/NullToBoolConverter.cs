using System.Globalization;
using System.Windows.Data;

namespace EasySoftware.WpfControls.Converters
{
    /// <summary>
    /// Converts <see langword="null"/> to <see langword="true"/> and any other value to <see langword="false"/>.
    /// </summary>
    public class NullToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Checks whether a value is <see langword="null"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is <see langword="null"/>; otherwise <see langword="false"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value == null;

        /// <summary>
        /// Not supported; always returns <see cref="Binding.DoNothing"/>.
        /// </summary>
        /// <param name="value">Not used.</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used.</param>
        /// <returns><see cref="Binding.DoNothing"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}
