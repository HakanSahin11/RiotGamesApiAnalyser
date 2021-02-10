using System;
using System.Collections.Generic;
using System.Text;

namespace Lol_Decay_Reminder.Models
{
    class MatchesModel
    {
        public MatchesModel(List<Match> matches)
        {
            this.matches = matches;
        }
        public List<Match> matches { get; set; }
    }
    public class Match:IMatch
    {
        public string platformId { get; set; }
        public double gameId { get; set; }
        public double champion { get; set; }
        public double queue { get; set; }
        public double season { get; set; }
        public long timestamp { get; set; }
        public string role { get; set; }
        public string lane { get; set; }
    }
    public interface IMatch
    {
       string platformId { get; set; }
       double gameId { get; set; }
       double champion { get; set; }
       double queue { get; set; }
       double season { get; set; }
       long timestamp { get; set; }
       string role { get; set; }
       string lane { get; set; }
    }
}
