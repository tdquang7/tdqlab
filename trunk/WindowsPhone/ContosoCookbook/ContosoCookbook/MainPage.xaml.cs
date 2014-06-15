using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ContosoCookbook.Resources;
using ContosoCookbook.Data;

namespace ContosoCookbook
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Microsoft.Phone.Shell.ProgressIndicator pi;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void lstGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstGroups.SelectedIndex > -1)
            {
                NavigationService.Navigate(new Uri("/GroupDetailPage.xaml?ID=" + (lstGroups.SelectedItem as RecipeDataGroup).UniqueId, UriKind.Relative));
            }
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

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.Recipes.IsLoaded)
            {
                pi = new Microsoft.Phone.Shell.ProgressIndicator();
                pi.IsIndeterminate = true;
                pi.Text = "Loading data, please wait...";
                pi.IsVisible = true;

                Microsoft.Phone.Shell.SystemTray.SetIsVisible(this, true);
                Microsoft.Phone.Shell.SystemTray.SetProgressIndicator(this, pi);

                await App.Recipes.LoadLocalDataAsync();

                lstGroups.DataContext = App.Recipes.ItemGroups;

                pi.IsVisible = false;
                Microsoft.Phone.Shell.SystemTray.SetIsVisible(this, false);

            }
            base.OnNavigatedTo(e);
        }
    }
}