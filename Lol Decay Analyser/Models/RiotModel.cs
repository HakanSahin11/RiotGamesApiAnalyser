using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lol_Decay_Analyser.Models
{
    public class RiotModel
    {
        public int Id { get; set; }
        public string SummonerName { get; set; }
        public string Rank { get; set; }
        public DateTime? LastMatch { get; set; }
        public int? TimeRemain { get; set; } 
    }
}
