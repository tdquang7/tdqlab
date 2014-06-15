using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using FileAssocDriver.Resources;
using Windows.Storage;
using System.IO;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace FileAssocDriver
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            if (local != null)
            {
                await WriteFiles();

                StorageFile recipeFile = await local.GetFileAsync("recipe.rcp");
                if (recipeFile != null)
                    await Windows.System.Launcher.LaunchFileAsync(recipeFile);
            }

        }

        private async Task WriteFiles()
        {
            StreamReader stream = new StreamReader(TitleContainer.OpenStream("sample.rcp"));

            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await local.CreateFileAsync("recipe.rcp", CreationCollisionOption.ReplaceExisting);

            string fileAsString = stream.ReadToEnd();
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(fileAsString);

            var outputStream = await file.OpenStreamForWriteAsync();
            outputStream.Write(fileBytes, 0, fileBytes.Length);

            stream.Close();
            outputStream.Close();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}