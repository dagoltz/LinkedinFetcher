using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkedinFetcher.Common.Interfaces;
using LinkedinFetcher.Common.Models;
using LinkedinFetcher.DataProvider.Store.Models;
using MongoDB.Driver;

namespace LinkedinFetcher.DataProvider.Store
{
    [ExcludeFromCodeCoverage]
    public class MongoProfileStore : LinqSearchStore
    {
        // TODO: get from config
        const string ConnectionString = "******";
        private const string DbName = "linkedin-fetcher-db";
        private const string CollectionName = "profiles";

        public override void Store(Profile profile)
        {
            var collection = GetMongoCollection();
            collection.InsertOne(new MongoProfile(profile));
        }

        public override IEnumerable<Profile> Search(SearchParameters parameters)
        {
            var collection = GetMongoCollection();
            var results = Search(parameters, collection.AsQueryable()).Select(p => new Profile(p));
            return results;
        }

        private IMongoCollection<MongoProfile> GetMongoCollection()
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(DbName);

            IMongoCollection<MongoProfile> collection = database.GetCollection<MongoProfile>(CollectionName);
            return collection;
        }
    }
}
