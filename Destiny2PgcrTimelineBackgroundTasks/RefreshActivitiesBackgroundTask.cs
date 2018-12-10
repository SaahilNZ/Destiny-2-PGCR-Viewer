using Destiny2PgcrTimeline.Shared;
using Destiny2PgcrTimeline.Shared.Services.Bungie;
using Destiny2PgcrTimeline.Shared.Services.MsGraph;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Notifications;

namespace Destiny2PgcrTimelineBackgroundTasks
{
    public sealed class RefreshActivitiesBackgroundTask : IBackgroundTask
    {
        BackgroundTaskDeferral deferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            deferral = taskInstance.GetDeferral();

            try
            {
                var connectionCost = NetworkInformation.GetInternetConnectionProfile().GetConnectionCost();

                if (NetworkInterface.GetIsNetworkAvailable()
                    && (connectionCost.NetworkCostType == NetworkCostType.Unknown
                    || connectionCost.NetworkCostType == NetworkCostType.Unrestricted))
                {
                    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

                    var player = JsonConvert.DeserializeObject<DestinyPlayerInformation>((string)localSettings.Values["player"]);

                    int platform = player.MembershipType;
                    string accountId = player.MembershipID;
                    int mode = 5;

                    var bungie = new BungieService(SharedData.BungieApiKey);
                    await bungie.DownloadDestinyManifest();

                    foreach (var characterId in player.CharacterIDs)
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
                }
            }
            catch (Exception ex)
            {
                ToastContent toastContent = new ToastContent()
                {
                    Launch = "refresh-activity",
                    Visual = new ToastVisual()
                    {
                        BindingGeneric = new ToastBindingGeneric()
                        {
                            Children =
                        {
                            new AdaptiveText
                            {
                                Text = "An error occured while refreshing Destiny 2 activity history"
                            }
                        }
                        }
                    }
                };

                var toast = new ToastNotification(toastContent.GetXml());
                toast.ExpirationTime = DateTime.Now.AddDays(2);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }

            deferral.Complete();
        }
    }
}
