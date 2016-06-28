using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkedinFetcher.Common.Interfaces;
using LinkedinFetcher.Common.Models;

namespace LinkedinFetcher.DataProvider.Store
{
    public class MongoProfileStore : IProfileStore
    {
        public void Store(Profile profile)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Profile> Search(SearchParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
