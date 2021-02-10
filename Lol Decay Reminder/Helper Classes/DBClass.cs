using System;
using System.Collections.Generic;
using System.Text;
using Lol_Decay_Reminder.Models;
using MongoDB.Driver;

namespace Lol_Decay_Reminder.Helper_Classes
{
   public class DBClass
    {
        private readonly IMongoCollection<DbUserModel> _collection;
        private readonly IDbContextSettings DbContext = new DbContext()
        {
            connectionString = "mongodb://adminUser:silvereye@localhost:27017",
            database = "RiotApi",
            collection = "DecayReminder"
        };
        public DBClass()
        {
            _collection = new MongoClient(DbContext.connectionString).GetDatabase(DbContext.database).GetCollection<DbUserModel>(DbContext.collection);
        }

        public List<DbUserModel> GetAll()
        {
            return _collection.Find(x => true).ToList();
        }
        public DbUserModel GetOne(string summonerName)
        {
            return _collection.Find<DbUserModel>(x => x.name == summonerName).FirstOrDefault();
        }
        public bool Create(DbUserModel user)
        {
            try
            {
                _collection.InsertOne(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Update(DbUserModel user)
        {
            _collection.ReplaceOne(x => x.name == user.name, user);
        }
        public void Delete(DbUserModel user)
        {
            _collection.DeleteOne(x => x.name == user.name);
        }

    }
}
