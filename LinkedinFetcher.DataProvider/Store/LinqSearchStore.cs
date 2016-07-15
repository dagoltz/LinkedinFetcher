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

        public IEnumerable<Profile> Search(SearchParameters parameters, IProfileRanker ranker)
        {
            return Search(parameters).OrderByDescending(p => ranker.Rank(p, parameters));
        }

        protected IEnumerable<Profile> Search(SearchParameters parameters, IEnumerable<Profile> profiles)
        {
            return profiles
                .Where(p => (p.Name ?? String.Empty).Contains(parameters.Name ?? String.Empty))
                .Where(p => (p.CurrentPosition ?? String.Empty).Contains(parameters.CurrentPosition ?? String.Empty))
                .Where(p => (p.CurrentTitle ?? String.Empty).Contains(parameters.CurrentTitle ?? String.Empty))
                .Where(p => (p.Summary ?? String.Empty).Contains(parameters.Summary ?? String.Empty))
                .Where(p => (parameters.Skills ?? Enumerable.Empty<string>()).All(s => p.Skills.Any(ps => ps.Contains(s))));
        }
    }
}
