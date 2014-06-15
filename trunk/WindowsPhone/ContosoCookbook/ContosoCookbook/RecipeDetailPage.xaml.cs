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
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using ContosoCookbook.Common;

namespace ContosoCookbook
{
    public partial class RecipeDetailPage : PhoneApplicationPage
    {
        private RecipeDataItem item;
        CameraCaptureTask camera;
        private const string removeFavUri = "/Assets/Icons/unlike.png";
        private const string FavUri = "/Assets/Icons/like.png";
        private const string removeAlarmUri = "/Assets/Icons/alarmRemove.png";
        private const string AlarmUri = "/Assets/Icons/alarm.png";

        public ApplicationBarIconButton pinBtn
        {
            get
            {
                var appBar = (ApplicationBar)ApplicationBar;
                var count = appBar.Buttons.Count;
                for (var i = 0; i < count; i++)
                {
                    ApplicationBarIconButton btn = appBar.Buttons[i] as ApplicationBarIconButton;
                    if (btn.IconUri.OriginalString.Contains("like"))
                        return btn;
                }
                return null;
            }
        }

        public ApplicationBarIconButton alarmBtn
        {
            get
            {
                var appBar = (ApplicationBar)ApplicationBar;
                var count = appBar.Buttons.Count;
                for (var i = 0; i < count; i++)
                {
                    ApplicationBarIconButton btn = appBar.Buttons[i] as ApplicationBarIconButton;
                    if (btn.IconUri.OriginalString.Contains("alarm"))
                        return btn;
                }
                return null;
            }
        }

        public RecipeDetailPage()
        {
            InitializeComponent();

            camera = new CameraCaptureTask();
            camera.Completed += camera_Completed;
        }

        private void camera_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);

                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isoStore.DirectoryExists(item.Group.Title))
                        isoStore.CreateDirectory(item.Group.Title);

                    string fileName = string.Format("{0}/{1}.jpg", item.Group.Title, DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss"));

                    using (IsolatedStorageFileStream targetStream = isoStore.CreateFile(fileName))
                    {
                        WriteableBitmap wb = new WriteableBitmap(bmp);
                        wb.SaveJpeg(targetStream, wb.PixelWidth, wb.PixelHeight, 0, 100);
                        targetStream.Close();
                    }

                    if (null == item.UserImages)
                        item.UserImages = new System.Collections.ObjectModel.ObservableCollection<string>();

                    item.UserImages.Add(fileName);
                }
            }
        }

        private void btnTakePicture_Click(object sender, EventArgs e)
        {
            try
            {
                camera.Show();
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show("An error occurred.");
            }
        }

        private void btnShareShareTask_Click(object sender, EventArgs e)
        {
            ShareMediaTask shareMediaTask = new ShareMediaTask();
            if (null != item.UserImages && item.UserImages.Count > 0)
                shareMediaTask.FilePath = string.Format("{0}", item.UserImages[0]);
            else
                shareMediaTask.FilePath = string.Format("{0}", item.GetImageUri());
            shareMediaTask.Show();
        }

        private void btnStartCooking_Click(object sender, EventArgs e)
        {
            Features.Notifications.SetReminder(item);
            SetScheduleBar(item.UniqueId);
        }

        private void btnPinToStart_Click(object sender, EventArgs e)
        {
            var uri = NavigationService.Source.ToString();
            if (Features.Tile.TileExists(uri))
                Features.Tile.DeleteTile(uri);
            else
                Features.Tile.SetTile(item, uri);

            SetPinBar();
        }

        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string UniqueId = "";

            UniqueId = NavigationContext.QueryString["ID"];

            if (!App.Recipes.IsLoaded)
                await App.Recipes.LoadLocalDataAsync();            

            NavigateToRecipe(UniqueId);
            SetPinBar();

            base.OnNavigatedTo(e);
        }

        void SetPinBar()
        {
            var uri = NavigationService.Source.ToString();
            if (Features.Tile.TileExists(uri))
            {
                pinBtn.IconUri = new Uri(removeFavUri, UriKind.Relative);
                pinBtn.Text = "Unpin";
            }
            else
            {
                pinBtn.IconUri = new Uri(FavUri, UriKind.Relative);
                pinBtn.Text = "Pin";
            }
        }

        private void NavigateToRecipe(string UniqueId)
        {
            item = App.Recipes.FindRecipe(UniqueId);
            pivot.DataContext = item;

            SetScheduleBar(item.UniqueId);
        }

        void SetScheduleBar(string name)
        {
            var isScheduled = Features.Notifications.IsScheduled(name);
            if (isScheduled)
            {
                alarmBtn.IconUri = new Uri(removeAlarmUri, UriKind.Relative);
                alarmBtn.Text = "Remove Alarm";
            }
            else
            {
                alarmBtn.IconUri = new Uri(AlarmUri, UriKind.Relative);
                alarmBtn.Text = "Set Alarm";
            }
        }
    }
}