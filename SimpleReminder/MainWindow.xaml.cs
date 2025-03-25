using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;

namespace SimpleReminder
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _reminderTimer;
        private DispatcherTimer _statusTimer;
        private List<Reminder> _reminders = new List<Reminder>();
        private string _settingsFile = "reminders.json";
        private DateTime _nextNotificationTime;

        public MainWindow()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;
            InitializeComponent();

            // Инициализация текстового поля с подсказкой
            NewReminderText.Foreground = Brushes.Gray;
            NewReminderText.Text = "Введите напоминание...";

            // Инициализация таймеров
            InitializeReminderTimer();
            InitializeStatusTimer();

            // Загрузка данных
            LoadReminders();
            UpdateRemindersList();
        }

        private void InitializeReminderTimer()
        {
            _reminderTimer = new DispatcherTimer();
            _reminderTimer.Interval = TimeSpan.FromSeconds(1);
            _reminderTimer.Tick += CheckReminders;
        }

        private void InitializeStatusTimer()
        {
            _statusTimer = new DispatcherTimer();
            _statusTimer.Interval = TimeSpan.FromSeconds(1);
            _statusTimer.Tick += UpdateStatusBar;
            _statusTimer.Start();
        }

        private void LoadReminders()
        {
            try
            {
                if (File.Exists(_settingsFile))
                {
                    string json = File.ReadAllText(_settingsFile);
                    _reminders = JsonConvert.DeserializeObject<List<Reminder>>(json) ?? new List<Reminder>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        private void SaveReminders()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_reminders, Formatting.Indented);
                File.WriteAllText(_settingsFile, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void UpdateRemindersList()
        {
            RemindersListBox.ItemsSource = null;
            RemindersListBox.ItemsSource = _reminders;
        }

        private void AddReminder_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewReminderText.Text) ||
                NewReminderText.Text == "Введите напоминание...")
            {
                MessageBox.Show("Введите текст напоминания!");
                return;
            }

            if (!int.TryParse(IntervalTextBox.Text, out int minutes) || minutes <= 0)
            {
                MessageBox.Show("Введите корректное число минут (больше 0)");
                IntervalTextBox.Focus();
                return;
            }

            _reminders.Add(new Reminder
            {
                Message = NewReminderText.Text,
                Interval = TimeSpan.FromMinutes(minutes),
                IsEnabled = true,
                LastShown = DateTime.MinValue
            });

            // Сброс полей ввода
            NewReminderText.Clear();
            NewReminderText.Foreground = Brushes.Gray;
            NewReminderText.Text = "Введите напоминание...";
            IntervalTextBox.Text = "30";

            UpdateRemindersList();
            SaveReminders();
        }

        private void DeleteReminder_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag is Reminder reminder)
            {
                _reminders.Remove(reminder);
                UpdateRemindersList();
                SaveReminders();
            }
        }

        private void StartReminders_Click(object sender, RoutedEventArgs e)
        {
            if (_reminderTimer.IsEnabled)
            {
                _reminderTimer.Stop();
                StartButton.Content = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Children =
                    {
                        new PackIcon { Kind = PackIconKind.Play, Width = 16, Height = 16, Margin = new Thickness(0,0,4,0) },
                        new TextBlock { Text = "СТАРТ" }
                    }
                };
            }
            else
            {
                _reminderTimer.Start();
                StartButton.Content = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Children =
                    {
                        new PackIcon { Kind = PackIconKind.Pause, Width = 16, Height = 16, Margin = new Thickness(0,0,4,0) },
                        new TextBlock { Text = "ПАУЗА" }
                    }
                };
                _nextNotificationTime = DateTime.Now.Add(_reminderTimer.Interval);
            }
        }

        private void CheckReminders(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            _nextNotificationTime = now.Add(_reminderTimer.Interval);

            foreach (var reminder in _reminders.Where(r => r.IsEnabled))
            {
                if ((now - reminder.LastShown) >= reminder.Interval)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        new NotificationWindow(reminder.Message, "").Show();
                    });
                    reminder.LastShown = now;
                }
            }
        }

        private void UpdateStatusBar(object sender, EventArgs e)
        {
            int activeCount = _reminders.Count(r => r.IsEnabled);
            StatusText.Text = $"Активных напоминаний: {activeCount}";

            if (_reminderTimer.IsEnabled)
            {
                var nextReminder = _reminders
                    .Where(r => r.IsEnabled)
                    .Select(r => r.LastShown.Add(r.Interval))
                    .OrderBy(t => t)
                    .FirstOrDefault();

                if (nextReminder > DateTime.Now)
                {
                    TimeSpan remaining = nextReminder - DateTime.Now;
                    TimeRemainingText.Text = $"Следующее через: {remaining:mm\\:ss}";
                }
                else
                {
                    TimeRemainingText.Text = "Ожидайте уведомления...";
                }
            }
            else
            {
                TimeRemainingText.Text = "Таймер остановлен";
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NewReminderText.Text == "Введите напоминание...")
            {
                NewReminderText.Text = "";
                NewReminderText.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewReminderText.Text))
            {
                NewReminderText.Text = "Введите напоминание...";
                NewReminderText.Foreground = Brushes.Gray;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        protected override void OnClosed(EventArgs e)
        {
            SaveReminders();
            _reminderTimer?.Stop();
            _statusTimer?.Stop();
            base.OnClosed(e);
        }
    }

    public class Reminder
    {
        public string Message { get; set; }
        public TimeSpan Interval { get; set; }
        public bool IsEnabled { get; set; } = true;
        public DateTime LastShown { get; set; }
    }
}