using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkedinFetcher.Common.Models;
using LinkedinFetcher.DataProvider.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkedinFetcher.Tests.DataProviders
{
    [TestClass]
    public class LinqSearchStoreTest
    {
        #region Init
        private LinqSearchStore _store;

        [TestInitialize]
        public void TestInit()
        {
            //Arrange.
            /* we can use memory store as the implimentation is in the base class */
            _store = new MemoryProfileStore();

            _store.Store(new Profile()
            {
                Name = "Daniel Goltz",
                Skills = new List<string>() { "C#", ".NET", "SQL"}
            });
        }


        #endregion
    }
}
