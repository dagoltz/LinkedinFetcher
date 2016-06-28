using System;

namespace LinkedinFetcher.DataProvider.Cache
{
    public interface ICacheProvider<T> where T : class
    {
        T GetCachedData(Func<T> getFreshData);
        T GetCachedData(Func<string, T> getFreshData, string entityId);
    }
}