using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasySoftware.WpfControls.Controls
{
    public class BusyIndicator : ContentControl
    {
        static BusyIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(BusyIndicator),
                new FrameworkPropertyMetadata(typeof(BusyIndicator)));
        }

        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register(nameof(IsBusy), typeof(bool), typeof(BusyIndicator), new PropertyMetadata(false));

        public double? Progress
        {
            get => (double?)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register(nameof(Progress), typeof(double?), typeof(BusyIndicator));

        public ICommand CancelCommand
        {
            get => (ICommand)GetValue(CancelCommandProperty);
            set => SetValue(CancelCommandProperty, value);
        }
        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(BusyIndicator));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(BusyIndicator),
                new PropertyMetadata("Please wait..."));


        public string ProgressMessage
        {
            get => (string)GetValue(ProgressMessageProperty);
            set => SetValue(ProgressMessageProperty, value);
        }
        public static readonly DependencyProperty ProgressMessageProperty =
            DependencyProperty.Register(nameof(ProgressMessage), typeof(string), typeof(BusyIndicator));
    }
}