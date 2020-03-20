using StackOfflineFlow.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StackOfflineFlow.Services
{
    public static class BulkDataFileService
    {
        public static BulkDataFiles GetBulkDataFilePathsFromDirectory(string directoryPath)
        {
            var returnValue = new BulkDataFiles();
            var directoryInfo = new DirectoryInfo(directoryPath);
            var xmlFiles = directoryInfo.GetFiles("*.xml"); //Getting Text files
            foreach (var file in xmlFiles)
            {
                if (String.Equals(file.Name, "Badges.xml", StringComparison.OrdinalIgnoreCase))
                {
                    returnValue.BadgesPath = file.FullName;
                }
                if (String.Equals(file.Name, "Comments.xml", StringComparison.OrdinalIgnoreCase))
                {
                    returnValue.CommentsPath = file.FullName;
                }
                if (String.Equals(file.Name, "PostHistory.xml", StringComparison.OrdinalIgnoreCase))
                {
                    returnValue.PostHistoryPath = file.FullName;
                }
                if (String.Equals(file.Name, "PostLinks.xml", StringComparison.OrdinalIgnoreCase))
                {
                    returnValue.PostLinksPath = file.FullName;
                }
                if (String.Equals(file.Name, "Posts.xml", StringComparison.OrdinalIgnoreCase))
                {
                    returnValue.PostsPath = file.FullName;
                }
                if (String.Equals(file.Name, "Tags.xml", StringComparison.OrdinalIgnoreCase))
                {
                    returnValue.TagsPath = file.FullName;
                }
                if (String.Equals(file.Name, "Users.xml", StringComparison.OrdinalIgnoreCase))
                {
                    returnValue.UsersPath = file.FullName;
                }
                if (String.Equals(file.Name, "Votes.xml", StringComparison.OrdinalIgnoreCase))
                {
                    returnValue.BadgesPath = file.FullName;
                }
            }

            return returnValue;
        }

    }
}
