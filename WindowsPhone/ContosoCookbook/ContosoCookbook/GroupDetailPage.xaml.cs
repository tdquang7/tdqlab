using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ContosoCookbook.Data;

namespace ContosoCookbook
{
    public partial class GroupDetailPage : PhoneApplicationPage
    {
        RecipeDataGroup group;

        public GroupDetailPage()
        {
            InitializeComponent();
        }

        private void lstRecipes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstRecipes.SelectedItems.Count > 0)
                NavigationService.Navigate(new Uri("/RecipeDetailPage.xaml?ID=" + (lstRecipes.SelectedItem as RecipeDataItem).UniqueId, UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string UniqueId = NavigationContext.QueryString["ID"];
            group = App.Recipes.FindGroup(UniqueId);
            pivot.DataContext = group;

            base.OnNavigatedTo(e);
        }
    }
}