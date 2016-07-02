using System;
using System.Collections.Generic;
using System.Linq;
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
                Skills = new List<string>() { "C#", ".NET", "SQL", "CSS", "OOP", "HTML", "JavaScript"},
                Summary = "Hi, I'm Daniel, a senior software developer. Feel free to visit my website to check out some of my latest work",
                CurrentPosition = "Israel Defense Forces",
                CurrentTitle = "Full-Stack .NET Developer",
                Groups = new List<string>() { "Geektime Friends", "AngularJS Developers", ".NET Developers in Israel"}
            });
            _store.Store(new Profile()
            {
                Name = "Tal Bronfer",
                Skills = new List<string>() { "Selenium", "C#", ".NET", "SQL", "CSS", "Web Applications", "JavaScript", "Node.js" },
                Summary = "Full-stack developer specializing in Web Apps. Keen to learn, well-motivated and passionate about technology, entrepreneurship and innovation.",
                CurrentPosition = "Gartner",
                CurrentTitle = "Software Developer at Gartner Innovation Center Israel",
                Groups = new List<string>() { "The 8200 developers group", "EcoMotion Israel", ".NET Experts in Israel", "Test Automation" }
            });
            _store.Store(new Profile()
            {
                Name = "Natali Nisim",
                Skills = new List<string>() { "C#", ".NET", "Java", "Linux", "CSS", "NLP", "Machine Learning", "Applied Mathematics" },
                CurrentPosition = "Dynamic Yield",
                CurrentTitle = "Software Developer",
                Groups = new List<string>() { "Natural Language Processing in Israel", "Microsoft Developers", "Israeli Hot Jobs Board" }
            });
        }
        #endregion

        [TestMethod]
        public void RegularInput_SearchByName()
        {
            // Arrange
            var parameters = new SearchParameters()
            {
                Name = "Tal"
            };

            // Act
            var results = _store.Search(parameters).ToList();

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());

            var result = results.First();
            Assert.AreEqual("Tal Bronfer", result.Name);
            Assert.AreEqual("Gartner", result.CurrentPosition);
        }

        [TestMethod]
        public void RegularInput_SearchByTitle()
        {
            // Arrange
            var parameters = new SearchParameters()
            {
                CurrentTitle = "Developer"
            };

            // Act
            var results = _store.Search(parameters).ToList();

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count());
            Assert.IsTrue(results.All(p => p.CurrentTitle.Contains("Developer")));
        }

        [TestMethod]
        public void RegularInput_SearchByPosition()
        {
            // Arrange
            var parameters = new SearchParameters()
            {
                CurrentPosition = "Israel Defense"
            };

            // Act
            var results = _store.Search(parameters).ToList();

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.IsTrue(results.All(p => p.Name == "Daniel Goltz"));
        }

        [TestMethod]
        public void RegularInput_SearchBySkills()
        {
            // Arrange
            var parameters = new SearchParameters()
            {
                Skills = new[] { ".NET" }
            };

            // Act
            var results = _store.Search(parameters).ToList();

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count());
            Assert.IsTrue(results.All(p => p.Skills.Contains(".NET")));
        }

        [TestMethod]
        public void RegularInput_SearchByMultiSkills()
        {
            // Arrange
            var parameters = new SearchParameters()
            {
                Skills = new[] { ".NET", "SQL" }
            };

            // Act
            var results = _store.Search(parameters).ToList();

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count());
            Assert.IsTrue(results.All(p => p.Skills.Contains(".NET") && p.Skills.Contains("SQL")));
        }

        [TestMethod]
        public void RegularInput_SearchBySummary()
        {
            // Arrange
            var parameters = new SearchParameters()
            {
                Summary = "software"
            };

            // Act
            var results = _store.Search(parameters).ToList();

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.IsTrue(results.All(p => p.Name == "Daniel Goltz"));
        }

        [TestMethod]
        public void RegularInput_SearchByMultiParams()
        {
            // Arrange
            var parameters = new SearchParameters()
            {
                CurrentTitle = "Israel",
                Summary = "developer",
                Skills = new[] { "C#", "CSS", "Node.js" }
            };

            // Act
            var results = _store.Search(parameters).ToList();

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.IsTrue(results.All(p => p.Name == "Tal Bronfer"));
        }

        [TestMethod]
        public void NotFoundInput_SearchByMultiParams()
        {
            // Arrange
            var parameters = new SearchParameters()
            {
                CurrentTitle = "Israel",
                Summary = "developer",
                Skills = new[] { "C#", "CSS", "Node.js", "NLP" }
            };

            // Act
            var results = _store.Search(parameters).ToList();

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }
    }
}
