using Avalonia.Controls;
using StackOfflineFlow.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StackOfflineFlow.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string BulkDataPath { get; set; }
        public string BadgesPath { get; set; }
        public string CommentsPath { get; set; }
        public string PostHistoryPath { get; set; }
        public string PostLinksPath { get; set; }
        public string PostsPath { get; set; }
        public string TagsPath { get; set; }
        public string UsersPath { get; set; }
        public string VotesPath { get; set; }
        public string SearchText { get; set; }
        public ObservableCollection<string> SearchResults { get; set; } = new ObservableCollection<string>();

        public MainWindowViewModel()
        {
            var appSettings = AutosaveService.GetAppSettings();
            BulkDataPath = "StackOverflow bulk data path: "+ appSettings.StackOverflowDataFolderPath;
            if(!String.IsNullOrEmpty(appSettings.StackOverflowDataFolderPath))
            {
                var paths = BulkDataFileService.GetBulkDataFilePathsFromDirectory(appSettings.StackOverflowDataFolderPath);
                BadgesPath = paths.BadgesPath;
                CommentsPath = paths.CommentsPath;
                PostHistoryPath = paths.PostHistoryPath;
                PostLinksPath = paths.PostLinksPath;
                PostsPath = paths.PostsPath;
                TagsPath = paths.TagsPath;
                UsersPath = paths.UsersPath;
                VotesPath = paths.VotesPath;
            }
        }

        public async Task OpenFile()
        {
            var dialog = new OpenFolderDialog();
            var result = await dialog.ShowAsync(new Window());

            if (!String.IsNullOrEmpty(result))
            {
                var appSettings = AutosaveService.GetAppSettings();
                appSettings.StackOverflowDataFolderPath = result;
                AutosaveService.SaveAppSettings(appSettings);
            }
        }

        public void Exit()
        {
            Environment.Exit(0);
        }

        public void Search()
        {
            var reader = XmlReader.Create(CommentsPath);
            var isInRow = false;
            SearchResults.Clear();
            while (reader.MoveToNextAttribute() || reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && String.Equals(reader.Name, "row"))
                {
                    isInRow = true;
                }
                if (reader.NodeType == XmlNodeType.Attribute && isInRow)
                {
                    if (reader.Name == "Text" && reader.Value.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    {
                        SearchResults.Add(reader.Value);
                        if (SearchResults.Count >= 100)
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
        }
    }
}
