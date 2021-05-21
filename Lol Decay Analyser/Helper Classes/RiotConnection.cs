using Lol_Decay_Analyser.Data;
using Lol_Decay_Analyser.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lol_Decay_Analyser.Helper_Classes
{
    public class RiotConnection
    {

        private readonly RiotContext _context;
        public RiotConnection(RiotContext context)
        {
            _context = context;
        }

        private readonly string _ApiKey = "RGAPI-307ab5f0-f415-40fe-8d3c-e9c1407ff655";

        //private readonly string _ApiKey = "###";


        private readonly List<string> ListOfRegions = new List<string> { "ALL", "EUW", "EUNE", "NA", "BR", "LAN", "LAS", "OCE", "RU", "TR", "JP", "KR" };

        private readonly List<RankedModel> ListOfRanks = new List<RankedModel>
            {
               new RankedModel("Diamond",    28 ,4),
               new RankedModel("Master",      10 ,10),
               new RankedModel("GrandMaster", 10,10 ),
               new RankedModel("Challenger",  10,10 )
            };

        public string ConvertRegion(string content)
        {
            
            // Converts regions to match Riot Devoloper APi Region requests
            string result = "";
            switch (content.ToUpper())
            {
                case "EUW":
                    result = "Euw1";
                    break;
                case "EUNE":
                    result = "Eun1";
                    break;
                case "NA":
                    result = "Na1";
                    break;
                case "BR":
                    result = "Br1";
                    break;
                case "LAN":
                    result = "La1";
                    break;
                case "LAS":
                    result = "La2";
                    break;
                case "OCE":
                    result = "Oc1";
                    break;
                case "RU":
                    result = "Ru1";
                    break;
                case "TR":
                    result = "Tr1";
                    break;
                case "JP":
                    result = "Jp1";
                    break;
                case "KR":
                    result = "Kr";
                    break;
            }
            return result;
        }

        public IUserModel GetAccountFromAPI(IRiotDBModel savedUsers, string region)
        {
            return JsonConvert.DeserializeObject<UserModel>(new WebClient().DownloadString($"https://{region}.api.riotgames.com/lol/summoner/v4/summoners/by-name/{savedUsers.SummonerName}?api_key={_ApiKey}"));
        }
        public List<Match> GetMatchesFromAPI(string accountId, string region)
        {
            return JsonConvert.DeserializeObject<MatchesModel>(new WebClient().DownloadString($"https://{region}.api.riotgames.com/lol/match/v4/matchlists/by-account/{accountId}?queue=420&api_key={_ApiKey}")).matches.Take(10).ToList();
        }
        public IRankModel GetRankFromAPI(string SummonerId, string region)
        {
            return JsonConvert.DeserializeObject<List<RankModel>>(new WebClient().DownloadString($"https://{region}.api.riotgames.com/lol/league/v4/entries/by-summoner/{SummonerId}?api_key={_ApiKey}"))[0];
        }

        public static DateTime UnixTimeToDateTime(long unixtime)
        {
            //Converts unix time to datetime format
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
            return dtDateTime;
        }

        public ListFormatterConfirm ListFormatter(string tier, List<Match> matches)
        {
            var ruleset = ListOfRanks.FirstOrDefault(x => x.Rank.Equals(tier, StringComparison.InvariantCultureIgnoreCase));
            bool validate = false;

            //Decay ranks
            if (ruleset != null)
            {
                var timeframe = DateTime.Now.AddDays(-ruleset.DayInterval);

                List<Match> formattedMatches = new List<Match>();
                formattedMatches.AddRange(matches.Where(x => UnixTimeToDateTime(x.timestamp) > timeframe).Take(ruleset.MinimumIntervalValue));
                
                //mail service?
                if (formattedMatches.Count > ruleset.MinimumIntervalValue || formattedMatches.Count == ruleset.MinimumIntervalValue)
                    validate = true;

                if (formattedMatches.Count != 0)
                {
                    return new ListFormatterConfirm( new ListFormatter(
                    formattedMatches,
                    UnixTimeToDateTime(formattedMatches.FirstOrDefault().timestamp),
                    UnixTimeToDateTime(formattedMatches.LastOrDefault().timestamp).AddDays(ruleset.DayInterval),
                    ruleset.MinimumIntervalValue - formattedMatches.Count), validate);
                }
                
                //Made for if decay is in progress
                else
                {
                    return
                        new ListFormatterConfirm(new ListFormatter(matches, UnixTimeToDateTime(matches.FirstOrDefault().timestamp), DateTime.Now, ruleset.MinimumIntervalValue), validate);
                }
            }
            // For ranks under the supported decay ranks
            else
                return new ListFormatterConfirm(new ListFormatter(matches, UnixTimeToDateTime(matches.FirstOrDefault().timestamp), null, 0), validate);
        }
        public RiotModel GetUserFromAPi(IRiotDBModel savedUser)
        {
            try
            {
                var convertedRegion = ConvertRegion(savedUser.Region);
                var User = GetAccountFromAPI(savedUser,convertedRegion);
                var rank = GetRankFromAPI(User.id, convertedRegion);
                var Matches = GetMatchesFromAPI(User.accountId, convertedRegion);
                var FormattedMatches = ListFormatter(rank.tier, Matches);

                // add section for games left, time remaining

                return new RiotModel {SummonerName = savedUser.SummonerName, Rank = $"{rank.tier} {rank.rank}", 
                    TimeRemain = FormattedMatches.ListFormatter.Timer, Region = savedUser.Region, 
                    Id = _context.Riot.Where(x => x.SummonerName == savedUser.SummonerName && x.Region == savedUser.Region).FirstOrDefault().Id , LastMatch = FormattedMatches.ListFormatter.LastMatch, RemainingGames = FormattedMatches.ListFormatter.GamesLeft };
            }
            catch
            {
                return null;
            }
        }      
    }
}
