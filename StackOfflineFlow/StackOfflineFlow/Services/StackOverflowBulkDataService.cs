using StackOfflineFlow.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace StackOfflineFlow.Services
{
    public class StackOverflowBulkDataService
    {
        private BulkDataFiles _BulkDataFiles;
        
        public StackOverflowBulkDataService(BulkDataFiles bulkDataFiles)
        {
            _BulkDataFiles = bulkDataFiles;
        }

        public List<string> FindMatchesInComments(string searchText, int limit = 100)
        {
            var reader = XmlReader.Create(_BulkDataFiles.CommentsPath);
            var isInRow = false;
            var resultList = new List<string>();
            while (reader.MoveToNextAttribute() || reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && String.Equals(reader.Name, "row"))
                {
                    isInRow = true;
                }
                if (reader.NodeType == XmlNodeType.Attribute && isInRow)
                {
                    if (reader.Name == "Text" && reader.Value.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    {
                        resultList.Add(reader.Value);
                        if (resultList.Count >= limit)
                        {
                            break;
                        }
                    }
                }
                if (reader.NodeType != XmlNodeType.Element && reader.NodeType != XmlNodeType.Attribute)
                {
                    isInRow = false;
                }
            }
            return resultList;
        }
    }
}
