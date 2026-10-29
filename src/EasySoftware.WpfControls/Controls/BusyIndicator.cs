using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasySoftware.WpfControls.Controls
{
    /// <summary>
    /// A content control that covers its content with a busy overlay while <see cref="IsBusy"/> is <see langword="true"/>.
    /// The overlay shows a <see cref="Message"/>, a progress bar, an optional <see cref="ProgressMessage"/> and an optional Cancel button.
    /// </summary>
    /// <example>
    /// <code language="xml">
    /// &lt;ctrl:BusyIndicator IsBusy="{Binding IsBusy}" Message="Loading..."&gt;
    ///     &lt;!-- your content --&gt;
    /// &lt;/ctrl:BusyIndicator&gt;
    /// </code>
    /// </example>
    public class BusyIndicator : ContentControl
    {
        static BusyIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(BusyIndicator),
                new FrameworkPropertyMetadata(typeof(BusyIndicator)));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the busy overlay is shown. The default is <see langword="false"/>.
        /// </summary>
        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }
        /// <summary>
        /// Identifies the <see cref="IsBusy"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register(nameof(IsBusy), typeof(bool), typeof(BusyIndicator), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the progress from 0 to 100. When it is <see langword="null"/>, the progress bar is indeterminate.
        /// </summary>
        public double? Progress
        {
            get => (double?)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }
        /// <summary>
        /// Identifies the <see cref="Progress"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register(nameof(Progress), typeof(double?), typeof(BusyIndicator));

        /// <summary>
        /// Gets or sets the command run by the Cancel button. When it is <see langword="null"/>, the button is hidden.
        /// </summary>
        public ICommand CancelCommand
        {
            get => (ICommand)GetValue(CancelCommandProperty);
            set => SetValue(CancelCommandProperty, value);
        }
        /// <summary>
        /// Identifies the <see cref="CancelCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(BusyIndicator));

        /// <summary>
        /// Gets or sets the message shown above the progress bar. The default is "Please wait...". When it is <see langword="null"/>, the message is hidden.
        /// </summary>
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }
        /// <summary>
        /// Identifies the <see cref="Message"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(BusyIndicator),
                new PropertyMetadata("Please wait..."));


        /// <summary>
        /// Gets or sets the text shown below the progress bar, for example "3 of 10". When it is <see langword="null"/>, the text is hidden.
        /// </summary>
        public string ProgressMessage
        {
            get => (string)GetValue(ProgressMessageProperty);
            set => SetValue(ProgressMessageProperty, value);
        }
        /// <summary>
        /// Identifies the <see cref="ProgressMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProgressMessageProperty =
            DependencyProperty.Register(nameof(ProgressMessage), typeof(string), typeof(BusyIndicator));
    }
}