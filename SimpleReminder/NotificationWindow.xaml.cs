using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace SimpleReminder
{
    public partial class NotificationWindow : Window
    {
        public NotificationWindow(string message, string customTitle = null)
        {
            InitializeComponent();

            MessageTextBlock.Text = message;

            if (!string.IsNullOrEmpty(customTitle))
            {
                CustomTitleTextBlock.Text = customTitle;
                CustomTitleTextBlock.Visibility = Visibility.Visible;
            }

            Loaded += OnWindowLoaded;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Left = SystemParameters.WorkArea.Width - ActualWidth - 10;
            Top = SystemParameters.WorkArea.Height - ActualHeight - 10;

            BeginAnimation(OpacityProperty,
                new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.2)));
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            var anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.2));
            anim.Completed += (s, _) => Close();
            BeginAnimation(OpacityProperty, anim);
        }
    }
}