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
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using ContosoCookbook.Data;
using Microsoft.Phone.Marketplace;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace ContosoCookbook.Common
{
    public class Features
    {
 
        public class Notifications
        {
            public static void SetReminder(RecipeDataItem item)
            {
                if (!IsScheduled(item.UniqueId))
                {
                    Microsoft.Phone.Scheduler.Reminder reminder = new Microsoft.Phone.Scheduler.Reminder(item.UniqueId);
                    reminder.Title = item.Title;
                    reminder.Content = "Have you finished cooking?";
                    if (System.Diagnostics.Debugger.IsAttached)
                        reminder.BeginTime = DateTime.Now.AddSeconds(10);
                    else
                        reminder.BeginTime = DateTime.Now.Add(TimeSpan.FromMinutes(Convert.ToDouble(item.PrepTime)));
                    reminder.ExpirationTime = reminder.BeginTime.AddSeconds(10);
                    reminder.RecurrenceType = RecurrenceInterval.None;
                    reminder.NavigationUri = new Uri("/RecipeDetailPage.xaml?ID=" + item.UniqueId + "&GID=" + item.Group.UniqueId, UriKind.Relative);
                    ScheduledActionService.Add(reminder);
                }
                else
                {
                    var schedule = ScheduledActionService.Find(item.UniqueId);
                    ScheduledActionService.Remove(schedule.Name);
                }
            }

            public static bool IsScheduled(string name)
            {
                var schedule = ScheduledActionService.Find(name);
                if (schedule == null)
                {
                    return false;
                }
                else
                {
                    return schedule.IsScheduled;
                }
            }
        }

        public class Tile
        {
            public static bool TileExists(string NavSource)
            {
                ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(o => o.NavigationUri.ToString().Contains(NavSource));
                return tile == null ? false : true;
            }

            public static void DeleteTile(string NavSource)
            {
                ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(o => o.NavigationUri.ToString().Contains(NavSource));
                if (tile == null) return;

                tile.Delete();
            }

            public static void SetTile(RecipeDataItem item, string NavSource)
            {
                StandardTileData tileData = new StandardTileData
                {
                    Title = "ContosoCookbook",
                    BackTitle = item.Group.Title,
                    BackContent = item.Title,
                    BackBackgroundImage = new Uri(item.Group.GetImageUri(), UriKind.Relative),
                    BackgroundImage = new Uri(item.GetImageUri(), UriKind.Relative)
                };
                ShellTile.Create(new Uri(NavSource, UriKind.Relative), tileData);
            }
        }
    }
}
