using Destiny2PgcrTimeline.Shared.Services.Bungie;
using Destiny2PgcrTimeline.Shared.Services.MsGraph;
using Destiny2PgcrTimeline.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AdaptiveCards.Rendering.Uwp;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using AdaptiveCards;
using Windows.UI;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Destiny2PgcrTimeline.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Destiny2PgcrTimeline
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel
        {
            get { return (MainViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MainViewModel),
                typeof(MainPage), new PropertyMetadata(null));

        public MainPage()
        {
            this.InitializeComponent();
            activityHistoryPane.ViewModel = new ActivityHistoryPaneViewModel();
            ViewModel = new MainViewModel(activityHistoryPane.ViewModel);
        }

        private async void btnSyncActivities_Click(object sender, RoutedEventArgs e)
        {
            //tbStatus.Text = "In progress...";

            try
            {
                var bungie = new BungieService(SharedData.BungieApiKey);

                int platform = 1;
                string accountName = "BlackDragon1999";

                var destinyPlayer = (await bungie.GetDestinyPlayers(platform, accountName)).First();
                var accountId = destinyPlayer.MembershipId;
                var destinyProfile = await bungie.GetDestinyProfile(platform, accountId);
                int mode = 5;

                await bungie.DownloadDestinyManifest();
                foreach (var characterId in destinyProfile.Data.CharacterIds)
                {
                    var history = await bungie.GetActivityHistory(platform, accountId, characterId, mode);
                    await Task.WhenAll(from pgcr in history
                                       select Task.Run(async () =>
                                       {
                                           var getActivityDefinition = bungie.GetActivityDefinitionAsync(pgcr.ActivityDetails.ReferenceId);
                                           var getModeDefinition = bungie.GetActivityModeDefinitionAsync(pgcr.ActivityDetails.Mode);

                                           var activity = new DestinyUserActivity(pgcr, await getActivityDefinition, await getModeDefinition, characterId);

                                           var msGraph = new MsGraphService(SharedData.MsGraphClientId);
                                           await msGraph.CreateOrReplaceActivityAsync(activity.Activity);
                                       }));
                }

                //tbStatus.Text = "Done";
            }
            catch (Exception ex)
            {
                //tbStatus.Text = $"Failed: {ex.ToString()}";
            }
        }

        private async void btnRegisterBackgroundTask_Click(object sender, RoutedEventArgs e)
        {
            var requestStatus = await BackgroundExecutionManager.RequestAccessAsync();
            var taskRegistered = false;
            var taskName = "RefreshActivitiesBackgroundTask";
            if (requestStatus == BackgroundAccessStatus.DeniedBySystemPolicy ||
                requestStatus == BackgroundAccessStatus.DeniedByUser)
            {
                //tbStatus.Text = $"Background tasks are not allowed: {requestStatus.ToString()}";
            }
            else
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        taskRegistered = true;
                        break;
                    }
                }

                if (taskRegistered)
                {
                    //tbStatus.Text = "Background task has already been registered";
                }
                else
                {
                    var builder = new BackgroundTaskBuilder
                    {
                        Name = taskName,
                        TaskEntryPoint = "Destiny2PgcrTimelineBackgroundTasks.RefreshActivitiesBackgroundTask"
                    };
                    builder.SetTrigger(new TimeTrigger(30, false));
                    BackgroundTaskRegistration task = builder.Register();

                    //tbStatus.Text = "Registered a background task";
                }
            }
        }

        private void btnUnregisterBackgroundTask_Click(object sender, RoutedEventArgs e)
        {
            var taskName = "RefreshActivitiesBackgroundTask";
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == taskName)
                {
                    task.Value.Unregister(true);
                    //tbStatus.Text = "Unregistered a background task";
                    break;
                }
            }
        }
        
        //private async void SignOutButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var accounts = await App.PublicClientApp.GetAccountsAsync();

        //    if (accounts.Any())
        //    {
        //        try
        //        {
        //            await App.PublicClientApp.RemoveAsync(accounts.FirstOrDefault());
        //            this.ResultText.Text = "User has signed-out";
        //            this.CallGraphButton.Visibility = Visibility.Visible;
        //            this.SignOutButton.Visibility = Visibility.Collapsed;
        //        }
        //        catch (MsalException ex)
        //        {
        //            ResultText.Text = $"Error signing-out user: {ex.Message}";
        //        }
        //    }
        //}
    }
}
