using System;
using System.Collections.Generic;
using System.Text;

namespace StackOfflineFlow.Models
{
    public class SearchResult<T>
    {
        public int RecordsSearchedCount { get; set; }
        public T Result { get; set; }
        public SearchUpdateStatusEnum UpdateStatus { get; set; }
    }
}
