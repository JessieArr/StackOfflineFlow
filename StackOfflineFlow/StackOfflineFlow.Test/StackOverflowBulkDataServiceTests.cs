using StackOfflineFlow.Services;
using StackOfflineFlow.Test.Helpers;
using System;
using Xunit;

namespace StackOfflineFlow.Test
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
        public void Test1()
        {
            var results = SUT.FindMatchesInComments("console");
            Assert.Equal(100, results.Count);
        }
    }
}
