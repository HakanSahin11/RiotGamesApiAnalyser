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

        //numb of api calls: 14

        //ENTER API KEY HERE
        private readonly string _ApiKey = "###";

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

        #region API Calls
        public IUserModel GetAccountFromAPI(IRiotDBModel savedUsers, string region)
        {
            return JsonConvert.DeserializeObject<UserModel>(new WebClient().DownloadString($"https://{region}.api.riotgames.com/lol/summoner/v4/summoners/by-name/{savedUsers.SummonerName}?api_key={_ApiKey}"));
        }
        public List<DateTime> GetMatchesFromAPI(string puuId, string region)
        {
            if (region.Equals("Euw1", StringComparison.OrdinalIgnoreCase) || region.Equals("Eun1", StringComparison.OrdinalIgnoreCase))
                region = "Europe";
            else if (region.Equals("Na1", StringComparison.OrdinalIgnoreCase))
                region = "Americas";
            else
                region = "Asia";
            var matchIds = JsonConvert.DeserializeObject<List<string>>(new WebClient().DownloadString($"https://{region}.api.riotgames.com/lol/match/v5/matches/by-puuid/{puuId}/ids?queue=420&start=0&count=11&api_key={_ApiKey}"));
            var matchList = new List<DateTime>();
            foreach (var item in matchIds)
            {
                var json = JsonConvert.DeserializeObject<Match>(new WebClient().DownloadString($"https://{region}.api.riotgames.com/lol/match/v5/matches/{item}?api_key={_ApiKey}"));
                var timer = UnixTimeToDateTime(json.info.gameCreation);
                matchList.Add(timer); 
            }

            return matchList;
        }
        public IRankModel GetRankFromAPI(string SummonerId, string region)
        {
            return JsonConvert.DeserializeObject<List<RankModel>>(new WebClient().DownloadString($"https://{region}.api.riotgames.com/lol/league/v4/entries/by-summoner/{SummonerId}?api_key={_ApiKey}"))[0];
        }

        #endregion

        #region API Formatter
        public static DateTime UnixTimeToDateTime(long unixtime)
        {
            //Converts unix time to datetime format
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
            return dtDateTime;
        }

        public ListFormatterConfirm ListFormatter(string tier, List<DateTime> matches)
        {
            var ruleset = ListOfRanks.FirstOrDefault(x => x.Rank.Equals(tier, StringComparison.InvariantCultureIgnoreCase));
            bool validate = false;

            //Decay ranks
            if (ruleset != null)
            {
                var timeframe = DateTime.Now.AddDays(-ruleset.DayInterval);

                List<DateTime> formattedMatches = new List<DateTime>();
            //    formattedMatches.AddRange(matches.Where(x => UnixTimeToDateTime(x.info.gameCreation) > timeframe).Take(ruleset.MinimumIntervalValue));
                formattedMatches.AddRange(matches.Where(x => x > timeframe).Take(ruleset.MinimumIntervalValue));


                               
                //mail service?
                if (formattedMatches.Count > ruleset.MinimumIntervalValue || formattedMatches.Count == ruleset.MinimumIntervalValue)
                    validate = true;

                if (formattedMatches.Count != 0)
                {
                    return new ListFormatterConfirm( new ListFormatter(
                    formattedMatches,
                //    UnixTimeToDateTime(formattedMatches.FirstOrDefault().info.gameCreation),
                    formattedMatches.FirstOrDefault(),
                  //  UnixTimeToDateTime(formattedMatches.LastOrDefault().info.gameCreation).AddDays(ruleset.DayInterval),
                    formattedMatches.LastOrDefault().AddDays(ruleset.DayInterval),
                    ruleset.MinimumIntervalValue - formattedMatches.Count), validate);
                }
                
                //Made for if decay is in progress
                else
                {
                    return
                        new ListFormatterConfirm(
                            new ListFormatter(
                                matches, matches.FirstOrDefault(), DateTime.Now, ruleset.MinimumIntervalValue), validate);
                }
            }
            // For ranks under the supported decay ranks
            else
                return new ListFormatterConfirm(new ListFormatter(matches, matches.FirstOrDefault(), null, 0), validate);
        }
        #endregion

        public RiotModel GetUserFromAPi(IRiotDBModel savedUser)
        {
            try
              {
                var convertedRegion = ConvertRegion(savedUser.Region);
                var User = GetAccountFromAPI(savedUser,convertedRegion);
                var rank = GetRankFromAPI(User.id, convertedRegion);                
                var MatchTimers = GetMatchesFromAPI(User.puuid, convertedRegion);
                var FormattedMatches = ListFormatter(rank.tier, MatchTimers);
                
                return new RiotModel {SummonerName = savedUser.SummonerName, Rank = $"{rank.tier} {rank.rank}", 
                    TimeRemain = FormattedMatches.ListFormatter.Timer, Region = savedUser.Region, 
                    Id = _context.Riot.Where(x => x.SummonerName == savedUser.SummonerName && x.Region == savedUser.Region).FirstOrDefault().Id , LastMatch = FormattedMatches.ListFormatter.LastMatch, RemainingGames = FormattedMatches.ListFormatter.GamesLeft };
            }
            catch (Exception e)
            {
                return null;
            }
        }      
    }
}
