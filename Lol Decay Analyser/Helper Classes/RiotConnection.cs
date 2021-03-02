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

        private readonly string _ApiKey = "###";

        private readonly List<string> ListOfRegions = new List<string> { "ALL", "EUW", "EUNE", "NA", "BR", "LAN", "LAS", "OCE", "RU", "TR", "JP", "KR" };

        private readonly List<RankedModel> ListOfRanks = new List<RankedModel>
            {
               new RankedModel("Diamond",    28 ,4),
               new RankedModel("Master",      7 ,10),
               new RankedModel("GrandMaster", 7,10 ),
               new RankedModel("Challenger",  7,10 )
            };

         public IUserModel GetAccountFromAPI(RiotModel savedUsers)
        {
            return JsonConvert.DeserializeObject<UserModel>(new WebClient().DownloadString($"https://{savedUsers.Region}.api.riotgames.com/lol/summoner/v4/summoners/by-name/{savedUsers.SummonerName}?api_key={_ApiKey}"));
        }
        public IMatch GetLastMatchFromAPI(RiotModel savedUser, string accountId)
        {
            return JsonConvert.DeserializeObject<MatchesModel>(new WebClient().DownloadString($"https://{savedUser.Region}.api.riotgames.com/lol/match/v4/matchlists/by-account/{accountId}?queue=420&api_key={_ApiKey}")).matches.FirstOrDefault();
        }
        public IRankModel GetRankFromAPI(RiotModel savedUsers, string SummonerId)
        {
            return JsonConvert.DeserializeObject<List<RankModel>>(new WebClient().DownloadString($"https://{savedUsers.Region}.api.riotgames.com/lol/league/v4/entries/by-summoner/{SummonerId}?api_key={_ApiKey}"))[0];
        }

        public static DateTime UnixTimeToDateTime(long unixtime)
        {
            //Converts unix time to datetime format
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
            return dtDateTime;
        }
        public RiotModel GetUserFromAPi(RiotModel savedUser)
        {
            try
            {
                var User = GetAccountFromAPI(savedUser);
                var LastMatch = GetLastMatchFromAPI(savedUser, User.accountId);
                var lastMatchTime = UnixTimeToDateTime(LastMatch.timestamp);
                var rank = GetRankFromAPI(savedUser, User.id);
                return new RiotModel {SummonerName = savedUser.SummonerName, LastMatch = lastMatchTime, Rank = $"{rank.tier} {rank.rank}", 
                    TimeRemain = null, Region = savedUser.Region, 
                    Id = _context.Riots.Where(x => x.SummonerName == savedUser.SummonerName && x.Region == savedUser.Region).FirstOrDefault().Id  };
            }
            catch
            {
                return null;
            }
        }

        //add region medifier
        public RiotModel RiotUser(List<string> savedUsers)
        {

            return new RiotModel();
        }
    }
}
