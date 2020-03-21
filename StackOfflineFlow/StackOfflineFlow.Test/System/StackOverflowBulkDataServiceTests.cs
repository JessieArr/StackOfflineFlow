using StackOfflineFlow.Services;
using StackOfflineFlow.Test.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace StackOfflineFlow.Test.System
{
    public class StackOverflowBulkDataServiceTests
    {
        private StackOverflowBulkDataService SUT;
        public StackOverflowBulkDataServiceTests()
        {
            var bulkDataFiles = BulkDataFilesHelper.Load();
            SUT = new StackOverflowBulkDataService(bulkDataFiles);
        }

        [Fact]
        public void FindMatchesInComments_ReturnsResults()
        {
            var results = SUT.FindMatchesInComments("console", x => {});
            Assert.Equal(100, results.Count);
        }

        [Fact]
        public void FindMatchesInComments_EmitsResults()
        {
            var emittedResults = new List<string>();
            var results = SUT.FindMatchesInComments("console", x => {
                emittedResults.Add(x);
            });
            Assert.Equal(100, emittedResults.Count);
        }

        [Fact]
        public void FindMatchesInPosts_ReturnsResults()
        {
            var results = SUT.FindMatchesInPosts("console", x => {}, 5);
            Assert.Equal(5, results.Count);
        }

        [Fact]
        public void FindMatchesInPosts_EmitsResults()
        {
            var emittedResults = new List<string>();

            var results = SUT.FindMatchesInPosts("console", x => {
                emittedResults.Add(x);
            }, 5);
            Assert.Equal(5, emittedResults.Count);
        }
    }
}
