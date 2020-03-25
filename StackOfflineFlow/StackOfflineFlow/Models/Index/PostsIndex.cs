using System;
using System.Collections.Generic;
using System.Text;

namespace StackOfflineFlow.Models.Index
{
    public class PostsIndex
    {
        public int TotalRecordCount { get; set; }
        public Dictionary<int, int> PostIDPositionsInXML { get; set; } = new Dictionary<int, int>();
    }
}
