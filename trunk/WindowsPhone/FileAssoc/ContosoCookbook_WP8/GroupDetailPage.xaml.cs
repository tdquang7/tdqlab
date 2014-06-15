// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using ContosoCookbook.Data;
using ContosoCookbook.Common;
using System.Windows.Navigation;

namespace ContosoCookbook
{
    public partial class GroupDetailPage : PhoneApplicationPage
    {
        RecipeDataGroup group;
        public GroupDetailPage()
        {
            InitializeComponent();
        }

        //protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        //{
        //    string UniqueId = NavigationContext.QueryString["ID"];
        //    group = App.Recipes.FindGroup(UniqueId);
        //    pivot.DataContext = group;

        //    base.OnNavigatedTo(e);
        //}

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.Recipes.IsLoaded)
                await App.Recipes.LoadLocalDataAsync();

            if (NavigationContext.QueryString.ContainsKey("groupName"))
            {
                string groupName = NavigationContext.QueryString["groupName"];

                group = App.Recipes.FindGroupByName(groupName);
                pivot.DataContext = group;
            }
            else
            {
                string UniqueId = NavigationContext.QueryString["ID"];
                group = App.Recipes.FindGroup(UniqueId);
                pivot.DataContext = group;
            }

            //SetPinBar();

            ////Update main tile with recently visited group
            //Features.Tile.UpdateMainTile(group);

            base.OnNavigatedTo(e);
        }


        private void lstRecipes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstRecipes.SelectedItems.Count > 0 && !group.LicensedRequired)
            {
                // + "&GID=" + (lstRecipes.SelectedItem as RecipeDataItem).Group.UniqueId, UriKind.Relative)
                NavigationService.Navigate(new Uri("/RecipeDetailPage.xaml?ID=" + (lstRecipes.SelectedItem as RecipeDataItem).UniqueId, UriKind.Relative));
            }
            else if (group.LicensedRequired)
            {
                var result = MessageBox.Show("Would you like to buy this product?", "Buy Product", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    group.LicensedRequired = false;
                    lstRecipes_SelectionChanged(sender, e);
                }
            }
        }
    }
}