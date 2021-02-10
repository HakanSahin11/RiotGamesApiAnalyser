using System;
using System.Collections.Generic;
using System.Text;

namespace Lol_Decay_Reminder.Models
{
    public class RankModel:IRankModel
    {
        public string leagueId { get; set; }
        public string queueType { get; set; }
        public string tier { get; set; }
        public string rank { get; set; }
        public string summonerId { get; set; }
        public string summonerName { get; set; }
        public int leaguePoints { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public bool veteran { get; set; }
        public bool inactive { get; set; }
        public bool freshBlood { get; set; }
        public bool hotStreak { get; set; }
    }
    public interface IRankModel
    {
       string leagueId { get; set; }
       string queueType { get; set; }
       string tier { get; set; }
       string rank { get; set; }
       string summonerId { get; set; }
       string summonerName { get; set; }
       int leaguePoints { get; set; }
       int wins { get; set; }
       int losses { get; set; }
       bool veteran { get; set; }
       bool inactive { get; set; }
       bool freshBlood { get; set; }
       bool hotStreak { get; set; }
    }
}
