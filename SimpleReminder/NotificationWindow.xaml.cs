using System.Windows;

namespace SimpleReminder
{
    public partial class NotificationWindow : Window
    {
        public NotificationWindow(string title, string message)
        {
            InitializeComponent();

            // Убедитесь, что имена совпадают с XAML!
            TitleTextBlock.Text = title;
            MessageTextBlock.Text = message;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}