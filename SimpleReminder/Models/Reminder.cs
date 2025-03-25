using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleReminder.Models
{
    public class Reminder
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public TimeSpan Interval { get; set; }
        public bool IsEnabled { get; set; } = true;
        public override string ToString()
        {
            return $"{Message} (каждые {Interval.TotalMinutes} мин)";
        }
    }

}
