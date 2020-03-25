using Newtonsoft.Json;
using StackOfflineFlow.Models;
using StackOfflineFlow.Services;
using StackOfflineFlow.Test.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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
            var results = SUT.FindMatchesInComments("console", x => { });
            Assert.Equal(100, results.Count);
        }

        [Fact]
        public void FindMatchesInComments_EmitsResults()
        {
            var emittedResults = new List<string>();
            var results = SUT.FindMatchesInComments("console", x =>
            {
                emittedResults.Add(x);
            });
            Assert.Equal(100, emittedResults.Count);
        }

        [Fact]
        public void FindMatchesInPosts_ReturnsResults()
        {
            var results = SUT.FindMatchesInPosts("console", x => { }, new CancellationToken());
            Assert.Equal(5, results.Count);
        }

        [Fact]
        public void FindMatchesInPosts_EmitsResults()
        {
            var emittedResults = new List<string>();

            var results = SUT.FindMatchesInPosts("console", x =>
            {
                emittedResults.Add(x.Result);
            }, new CancellationToken());
            Assert.Equal(5, emittedResults.Count);
        }

        [Fact]
        public void GetPostByID_DoesNotThrow()
        {
            var results = SUT.GetPostByID(4, 0, 100);
            Assert.NotNull(results);
        }

        [Fact]
        public void GetPostByID_FindsLargeIDsFaster()
        {
            var result1 = SUT.GetPostByID(100001, 0, 100000);
            var result2 = SUT.GetPostByID(60472846, 9575982, 100);
            Assert.NotNull(result1);
            Assert.NotNull(result2);
        }

        [Fact]
        public void GetAllElementPositionsByID_DoesNotThrow()
        {
            var result1 = SUT.GeneratePostsIndex();
            Assert.NotNull(result1);
            var serialized = JsonConvert.SerializeObject(result1);
            File.WriteAllText("index.json", serialized);
        }
    }
}
