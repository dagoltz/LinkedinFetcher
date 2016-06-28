using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkedinFetcher.Common.Interfaces;
using LinkedinFetcher.Common.Models;
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
            var collection = getMongoCollection();
            collection.InsertOne(profile);
        }

        public override IEnumerable<Profile> Search(SearchParameters parameters)
        {
            var collection = getMongoCollection();
            return Search(parameters, collection.AsQueryable());
        }

        private IMongoCollection<Profile> getMongoCollection()
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(DbName);

            IMongoCollection<Profile> collection = database.GetCollection<Profile>(CollectionName);
            return collection;
        }
    }
}
