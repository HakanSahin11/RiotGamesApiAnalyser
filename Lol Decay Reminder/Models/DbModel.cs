using System;
using System.Collections.Generic;
using System.Text;

namespace Lol_Decay_Reminder.Models
{
    public class DbContext : IDbContextSettings
    {
        public string connectionString { get; set; }
        public string database { get; set; }
        public string collection { get; set; }
    }
    public interface IDbContextSettings
    {
        string connectionString { get; set; }
        string database { get; set; }
        string collection { get; set; }
    }
}
