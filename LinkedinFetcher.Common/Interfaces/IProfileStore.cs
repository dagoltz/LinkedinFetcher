using System.Collections.Generic;
using LinkedinFetcher.Common.Models;

namespace LinkedinFetcher.Common.Interfaces
{
    public interface IProfileStore
    {
        void Store(Profile profile);
        IEnumerable<Profile> Search(SearchParameters parameters);
    }
}
