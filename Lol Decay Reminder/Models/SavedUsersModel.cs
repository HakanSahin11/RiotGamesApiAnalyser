using System;
using System.Collections.Generic;
using System.Text;

namespace Lol_Decay_Reminder.Models
{
   public class SavedUsersModel
    {
        public SavedUsersModel(string name, string region)
        {
            Name = name;
            Region = region;
        }

        public string Name { get; set; }
        public string Region { get; set; }
    }
}
