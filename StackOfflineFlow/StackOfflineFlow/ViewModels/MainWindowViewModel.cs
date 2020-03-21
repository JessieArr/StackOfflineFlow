using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;
using StackOfflineFlow.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        private string searchStatus;
        public string SearchStatus
        {
            get => searchStatus;
            set => this.RaiseAndSetIfChanged(ref searchStatus, value);
        }
        public ObservableCollection<string> SearchResults { get; set; } = new ObservableCollection<string>();

        private StackOverflowBulkDataService _BulkDataService;

        public MainWindowViewModel()
        {
            var appSettings = AutosaveService.GetAppSettings();
            BulkDataPath = "StackOverflow bulk data path: "+ appSettings.StackOverflowDataFolderPath;
            if(!String.IsNullOrEmpty(appSettings.StackOverflowDataFolderPath))
            {
                if(!Directory.Exists(appSettings.StackOverflowDataFolderPath))
                {
                    return;
                }
                var paths = BulkDataFileService.GetBulkDataFilePathsFromDirectory(appSettings.StackOverflowDataFolderPath);
                _BulkDataService = new StackOverflowBulkDataService(paths);
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

        public Task Search()
        {
            SearchStatus = "Searching...";
            return Task.Factory.StartNew(async () =>
            {
                
                var reader = XmlReader.Create(CommentsPath);
                var isInRow = false;
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    SearchResults.Clear();
                });

                _BulkDataService.FindMatchesInPosts(SearchText, async x =>
                {
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        SearchResults.Add(x);
                    });
                }, 10);

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    SearchStatus = $"Search complete - {SearchResults.Count} results found.";
                });
            });            
        }

        public void WhereIGetData()
        {
            var url = "https://archive.org/download/stackexchange";
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
