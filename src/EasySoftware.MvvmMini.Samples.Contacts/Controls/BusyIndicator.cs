using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EasySoftware.MvvmMini.Samples.Contacts.Controls
{
   public class BusyIndicator : Grid
   {
      private ProgressBar _progressBar;
      private Button _cancelButton;
      private Grid _busyContent;


      protected Grid BusyContent
      {
         get
         {
            if (this._busyContent == null)
               this._busyContent = GenerateBusyContent();
            return this._busyContent;
         }
      }

      private Grid GenerateBusyContent()
      {
         Grid grid = new Grid() { Background = new SolidColorBrush(Colors.White) { Opacity = 0.5 } };
         grid.RowDefinitions.Add(new RowDefinition { Height = new System.Windows.GridLength(1, GridUnitType.Star) });
         grid.RowDefinitions.Add(new RowDefinition { Height = new System.Windows.GridLength(60, GridUnitType.Pixel) });
         grid.RowDefinitions.Add(new RowDefinition { Height = new System.Windows.GridLength(1, GridUnitType.Star) });
         grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
         grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star), MinWidth = 140 });
         grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
         StackPanel panel = new StackPanel
         {
            Orientation = Orientation.Vertical,
            Background = new SolidColorBrush(Colors.White) { Opacity = 0.8 },
         };
         panel.Children.Add(new Label
         {
            HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
            Content = "Please wait...",
            Height = 34
         });
         _progressBar = new ProgressBar() { IsIndeterminate = true, Height = 15, Margin = new Thickness(8, 0, 8, 0) };
         panel.Children.Add(this._progressBar);

         grid.Children.Add(panel);
         Grid.SetRow(panel, 1);
         Grid.SetColumn(panel, 1);

         return grid;
      }


      #region IsBusy

      public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
          "IsBusy",
          typeof(bool),
          typeof(BusyIndicator),
          new PropertyMetadata(false, new PropertyChangedCallback(OnIsBusyChanged)));

      public bool IsBusy
      {
         get { return (bool)GetValue(IsBusyProperty); }
         set { SetValue(IsBusyProperty, value); }
      }

      private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         ((BusyIndicator)d).OnIsBusyChanged(e);
      }

      protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
      {
         if (IsBusy)
         {

            this.Children.Add(this.BusyContent);
            this._progressBar.IsIndeterminate = true;
         }
         else
             if (this.BusyContent != null)
         {
            this.Children.Remove(this.BusyContent);
            this._progressBar.IsIndeterminate = false;
         }
      }

      #endregion

      #region ShowCancelButton
      public static readonly DependencyProperty ShowCancelButtonProperty = DependencyProperty.Register(
          "ShowCancelButton",
          typeof(bool),
          typeof(BusyIndicator),
          new PropertyMetadata(false, new PropertyChangedCallback(ShowCancelButtonPropertyChanged)));

      public bool ShowCancelButton
      {
         get { return (bool)GetValue(ShowCancelButtonProperty); }
         set { SetValue(ShowCancelButtonProperty, value); }
      }

      private static void ShowCancelButtonPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         ((BusyIndicator)d).ShowCancelButtonPropertyChanged(e);
      }


      protected virtual void ShowCancelButtonPropertyChanged(DependencyPropertyChangedEventArgs e)
      {
         bool showButton = (bool)e.NewValue;
         StackPanel sp = this._busyContent.Children[0] as StackPanel;
         if (showButton)
         {
            if (sp.Children.Count == 2)
            {
               this.BusyContent.RowDefinitions[1].Height = new GridLength(100);
               this._cancelButton = new Button { Content = "Cancel", Margin = new Thickness(5) };
               this._cancelButton.Command = this.CancelCommand;
               sp.Children.Add(this._cancelButton);
            }
         }
         else
         {
            if (sp.Children.Count == 3)
            {
               sp.Children.Remove(sp.Children[2]);
            }
         }
      }


      #endregion


      #region CancelCommand
      public ICommand CancelCommand
      {
         get { return (ICommand)GetValue(CancelCommandProperty); }
         set { SetValue(CancelCommandProperty, value); }
      }

      public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(
          "CancelCommand",
          typeof(ICommand),
          typeof(BusyIndicator),
          new UIPropertyMetadata(null));

      #endregion




   }
}
