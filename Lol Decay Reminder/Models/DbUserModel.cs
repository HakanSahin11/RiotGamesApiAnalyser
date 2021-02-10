using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lol_Decay_Reminder.Models
{
    public class DbUserModel
    {
        public DbUserModel(ObjectId _id, string name, string region, double summonerLvl, string role, DateTime lastMatch, string id, string accountId, string puuid, double gameId, string rank, string tier)
        {
            this._id = _id;
            this.name = name;
            this.region = region;
            this.summonerLvl = summonerLvl;
            this.role = role;
            this.lastMatch = lastMatch;
            this.id = id;
            this.accountId = accountId;
            this.puuid = puuid;
            this.gameId = gameId;
            this.rank = rank;
            this.tier = tier;
        }

        [BsonId]
        public ObjectId _id         {get;set;}
        public string name          {get;set;}
        public string region { get; set; }
        public double summonerLvl      {get;set;}
        public string role          {get;set;}
        public DateTime lastMatch { get; set; }
        public string id { get; set; }
        public string accountId { get; set; }
        public string puuid { get; set; }
        public double gameId { get; set; }
        public string rank { get; set; }
        public string tier { get; set; }
    }
}
