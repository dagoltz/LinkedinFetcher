using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkedinFetcher.Common.Interfaces;
using LinkedinFetcher.Common.Models;

namespace LinkedinFetcher.DataProvider.Store
{
    public abstract class LinqSearchStore : IProfileStore
    {
        public abstract void Store(Profile profile);
        public abstract IEnumerable<Profile> Search(SearchParameters parameters);

        protected IEnumerable<Profile> Search(SearchParameters parameters, IEnumerable<Profile> profiles)
        {
            return profiles
                .Where(p => p.Name.Contains(parameters.Name ?? String.Empty))
                .Where(p => p.CurrentPosition.Contains(parameters.CurrentPosition ?? String.Empty))
                .Where(p => p.CurrentTitle.Contains(parameters.CurrentTitle ?? String.Empty))
                .Where(p => p.Summary.Contains(parameters.Summary ?? String.Empty))
                .Where(p => (parameters.Skills ?? Enumerable.Empty<string>()).All(s => p.Skills.Any(ps => ps.Contains(s))));
        }
    }
}
