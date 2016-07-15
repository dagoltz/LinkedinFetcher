using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkedinFetcher.Common.Models;

namespace LinkedinFetcher.Common.Interfaces
{
    public interface IProfileRanker
    {
        int Rank(Profile profile, SearchParameters parameters);
    }
}
