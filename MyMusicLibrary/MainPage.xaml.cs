﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
            //Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);
            //File is copied to local folder for use in music library
            //Album name and Song Title are displayed
            if (musicFolder != null && file != null)
            {
                var newFile = await file.CopyAsync(musicFolder, file.Name, NameCollisionOption.GenerateUniqueName);

                mediaPlayer.Source = MediaSource.CreateFromStorageFile(newFile);
                mediaPlayer.MediaPlayer.Play();

                var albumCoverBitmapImage = new BitmapImage();
                using (StorageItemThumbnail thumbnail = await newFile.GetThumbnailAsync(ThumbnailMode.MusicView, 300))
                    if (thumbnail !=null && thumbnail.Type == ThumbnailType.Image)
                    {
                        albumCoverBitmapImage.SetSource(thumbnail);
                        albumCover.Source = albumCoverBitmapImage;
                    };
                MusicProperties musicProperties = await newFile.Properties.GetMusicPropertiesAsync();
                artist.Text = musicProperties.Artist;
                title.Text = musicProperties.Title;
            }

            //Desiree: Here's the part I found for the Album and
            //StringBuilder outputText = new StringBuilder();
            //get music properties
            //
            //outputText.AppendLine("Album: " + musicProperties.Album);
            //outputText.AppendLine("Title: " + musicProperties.Title);
            //outputText.AppendLine("Artist: " + musicProperties.Artist);
            //MusicTextFiled.Text = outputText.ToString();


        }


    }
}
    

