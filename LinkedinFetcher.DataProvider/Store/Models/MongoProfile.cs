using LinkedinFetcher.Common.Models;
using MongoDB.Bson;

namespace LinkedinFetcher.DataProvider.Store.Models
{
    internal class MongoProfile : Profile
    {
        public ObjectId Id { get; set; }

        public MongoProfile()
        {
        }

        public MongoProfile(Profile otherProfile) : base(otherProfile)
        {
        }
    }
}