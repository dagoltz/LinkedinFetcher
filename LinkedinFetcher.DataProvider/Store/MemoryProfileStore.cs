using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkedinFetcher.Common.Interfaces;
using LinkedinFetcher.Common.Models;

namespace LinkedinFetcher.DataProvider.Store
{
    public class MemoryProfileStore : IProfileStore
    {
        private readonly List<Profile>  _profiles = new List<Profile>();

        public void Store(Profile profile)
        {
            _profiles.Add(profile);
        }

        public IEnumerable<Profile> Search(SearchParameters parameters)
        {
            return _profiles
                .Where(p => p.Name.Contains(parameters.Name ?? String.Empty))
                .Where(p => p.CurrentPosition.Contains(parameters.CurrentPosition ?? String.Empty))
                .Where(p => p.CurrentTitle.Contains(parameters.CurrentTitle ?? String.Empty))
                .Where(p => p.Summary.Contains(parameters.Summary ?? String.Empty))
                .Where(p => (parameters.Skills ?? Enumerable.Empty<string>()).All(s => p.Skills.Any(ps => ps.Contains(s))));
        }
    }
}
