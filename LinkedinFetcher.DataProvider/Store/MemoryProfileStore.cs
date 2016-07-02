using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkedinFetcher.Common.Interfaces;
using LinkedinFetcher.Common.Models;

namespace LinkedinFetcher.DataProvider.Store
{
    public class MemoryProfileStore : LinqSearchStore
    {
        private readonly List<Profile>  _profiles = new List<Profile>();

        public override void Store(Profile profile)
        {
            _profiles.Add(profile);
        }

        public override IEnumerable<Profile> Search(SearchParameters parameters)
        {
            return Search(parameters, _profiles);
        }
    }
}
