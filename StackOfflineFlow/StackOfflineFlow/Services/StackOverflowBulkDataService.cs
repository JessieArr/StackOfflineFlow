using StackOfflineFlow.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        public List<string> FindMatchesInComments(string searchText, Action<string> emitResult, int limit = 100)
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
                        emitResult(reader.Value);
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

        public List<string> FindMatchesInPosts(string searchText, Action<string> emitResult, int limit = 100)
        {
            var fileStream = new FileStream(_BulkDataFiles.PostsPath, FileMode.Open, FileAccess.Read);
            var watcher = new StreamWatcher(fileStream);
            var reader = XmlReader.Create(watcher);
            var resultList = new List<string>();
            while (reader.ReadToFollowing("row"))
            {
                if(fileStream.Position < 32000)
                {
                    continue;
                }
                var rowAttributes = new Dictionary<string, string>();
                while (reader.MoveToNextAttribute())
                {
                    rowAttributes.Add(reader.Name, reader.Value);
                }
                if (rowAttributes.ContainsKey("Body") && rowAttributes["Body"].Contains(""))
                {
                    resultList.Add(rowAttributes["Body"]);
                    emitResult(rowAttributes["Body"]);
                    if(resultList.Count >= limit)
                    {
                        return resultList;
                    }
                }
            }
            return resultList;
        }

        public Dictionary<string, string> GetPostByID(int id, int startPosition, int maxAttempts)
        {
            var fileStream = new FileStream(_BulkDataFiles.PostsPath, FileMode.Open, FileAccess.Read);
            var watcher = new StreamWatcher(fileStream);
            var byteStartPosition = (long)8192 * startPosition;
            fileStream.Position = byteStartPosition;
            var reader = XmlReader.Create(watcher, new XmlReaderSettings()
            {
                ValidationType = ValidationType.None,
                ConformanceLevel = ConformanceLevel.Fragment
                
            });
            var attemptCount = 0;
            while (reader.ReadToFollowing("row"))
            {                
                var rowAttributes = new Dictionary<string, string>();
                while (reader.MoveToNextAttribute())
                {
                    rowAttributes.Add(reader.Name, reader.Value);
                }
                if (rowAttributes.ContainsKey("Id") && rowAttributes["Id"] == id.ToString())
                {
                    var position = fileStream.Position / 8192;
                    return rowAttributes;
                }
                else
                {
                    attemptCount++;
                    if(attemptCount >= maxAttempts)
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        public Dictionary<int, int> GetAllElementPositionsByID()
        {
            var fileStream = new FileStream(_BulkDataFiles.PostsPath, FileMode.Open, FileAccess.Read);
            var reader = XmlReader.Create(fileStream);
            var results = new Dictionary<int, int>();
            while (reader.ReadToFollowing("row"))
            {
                while (reader.MoveToNextAttribute())
                {
                    if(reader.Name == "Id")
                    {
                        var position = (int)fileStream.Position / 8192;
                        results.Add(Int32.Parse(reader.Value), position);
                        continue;
                    }
                }
            }
            return results;
        }
    }
}
