using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyMusicLibrary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

 //Music Library opened, file picked, file copied in "musicfolder" in apps local folder
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation =
            Windows.Storage.Pickers.PickerLocationId.MusicLibrary
            };

            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".m4a");

            var file = await picker.PickSingleFileAsync();
            var folder = ApplicationData.Current.LocalFolder;
            var musicFolder = await folder.CreateFolderAsync("musicfolder", CreationCollisionOption.OpenIfExists);

            //put file in future access list so it can be accessed when application is closed and reopened
            Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);
            //File is copied to local folder for use in music library
            if (folder != null && file != null)
            {
                await file.CopyAsync(musicFolder, file.Name, NameCollisionOption.GenerateUniqueName);
            };

            //set media player source to file and play
            mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);
            mediaPlayer.MediaPlayer.Play();
        }


    }
}
    

