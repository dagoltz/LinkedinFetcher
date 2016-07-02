using LinkedinFetcher.Common.Models;

namespace LinkedinFetcher.Common.Interfaces
{
    public interface IProfileProvider
    {
        Profile GetProfile(string profileUrl);
    }
}
