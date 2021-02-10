using System;
using System.Collections.Generic;
using System.Text;

namespace Lol_Decay_Reminder.Models
{
    public interface IUserModel
    {
        string id { get; set; }
        string accountId { get; set; }
        string puuid { get; set; }
        string name { get; set; }
        double profileIconId { get; set; }
        double revisionDate { get; set; }
        double summonerLevel { get; set; }
    }
    public class UserModel : IUserModel
    {
        public string id { get; set; }
        public string accountId { get; set; }
        public string puuid { get; set; }
        public string name { get; set; }
        public double profileIconId { get; set; }
        public double revisionDate { get; set; }
        public double summonerLevel { get; set; }
       
    }
}
