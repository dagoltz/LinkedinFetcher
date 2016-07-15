using LinkedinFetcher.Common.Interfaces;
using LinkedinFetcher.Common.Models;

namespace LinkedinFetcher.DataProvider.Store
{
    public class SimpleProfileRanker : IProfileRanker
    {
        public int Rank(Profile profile, SearchParameters parameters)
        {
            int rank = 0;

            rank += profile.Skills.Count;
            rank += profile.Recommendations.Count;

            return rank;
        }
    }
}
