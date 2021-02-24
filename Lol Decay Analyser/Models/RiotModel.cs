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
        public DateTime? LastMatch { get; set; }
        public string Rank { get; set; }
        public int? TimeRemain { get; set; }
        public string Region { get; set; }
    }
    public class RankedModel
    {
        public RankedModel(string rank, int dayInterval, int minimumIntervalValue)
        {
            Rank = rank;
            DayInterval = dayInterval;
            MinimumIntervalValue = minimumIntervalValue;
        }

        public string Rank { get; set; }
        public int DayInterval { get; set; }
        public int MinimumIntervalValue { get; set; }
    }
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
    class MatchesModel
    {
        public MatchesModel(List<Match> matches)
        {
            this.matches = matches;
        }
        public List<Match> matches { get; set; }
    }
    public class Match : IMatch
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

    public class RankModel : IRankModel
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
