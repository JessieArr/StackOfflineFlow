using Newtonsoft.Json;
using StackOfflineFlow.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StackOfflineFlow.Test.Helpers
{
    public static class BulkDataFilesHelper
    {
        public static BulkDataFiles Load()
        {
            var text = File.ReadAllText("BulkDataFiles.json");
            return JsonConvert.DeserializeObject<BulkDataFiles>(text);
        }
    }
}
