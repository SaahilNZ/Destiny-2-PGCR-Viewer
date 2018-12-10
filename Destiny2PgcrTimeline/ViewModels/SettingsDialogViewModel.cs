using Destiny2PgcrTimeline.Shared;
using Destiny2PgcrTimeline.Shared.Services.Bungie;
using Destiny2PgcrTimeline.Shared.Services.MsGraph;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace Destiny2PgcrTimeline.ViewModels
{
    internal class SettingsDialogViewModel : ViewModelBase
    {
        private string playerName;
        private bool isVisible;
        private DestinyPlayerData playerData;
        private bool isSyncIdle;
        private bool isTaskRegistered;
        private bool isRegisterIdle;

        public string PlayerName
        {
            get { return playerName; }
            set
            {
                playerName = value;
                NotifyPropertyChanged(nameof(PlayerName));
            }
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                NotifyPropertyChanged(nameof(IsVisible));
            }
        }
        
        public DestinyPlayerData PlayerData
        {
            get { return playerData; }
            set
            {
                playerData = value;
                NotifyPropertyChanged(nameof(PlayerData));
            }
        }

        public bool IsSyncIdle
        {
            get { return isSyncIdle; }
            set
            {
                isSyncIdle = value;
                NotifyPropertyChanged(nameof(IsSyncIdle));
            }
        }

        public bool IsTaskRegistered
        {
            get { return isTaskRegistered; }
            set
            {
                isTaskRegistered = value;
                NotifyPropertyChanged(nameof(IsTaskRegistered));
                NotifyPropertyChanged(nameof(BackgroundSyncText));
            }
        }

        public bool IsRegisterIdle
        {
            get { return isRegisterIdle; }
            set
            {
                isRegisterIdle = value;
                NotifyPropertyChanged(nameof(IsRegisterIdle));
            }
        }

        public string BackgroundSyncText => IsTaskRegistered ? "Disable Background Sync" : "Enable Background Sync";

        public SettingsDialogViewModel()
        {
            IsSyncIdle = true;
            IsRegisterIdle = true;
            if (isBackgroundTaskRegistered("RefreshActivitiesBackgroundTask"))
            {
                IsTaskRegistered = true;
            }
        }

        public void CloseDialog()
        {
            IsVisible = false;
        }

        public async Task SyncToTimeline()
        {
            if (PlayerData != null)
            {
                IsSyncIdle = false;
                var bungie = new BungieService(Shared.SharedData.BungieApiKey);
                for (int i = 0; i < PlayerData.ActivityHistoryLists.Count; i++)
                {
                    await Task.WhenAll(from pgcr in PlayerData.ActivityHistoryLists[i]
                                       select Task.Run(async () =>
                                       {
                                           var getActivityDefinition = bungie.GetActivityDefinitionAsync(pgcr.Pgcr.ActivityDetails.ReferenceId);
                                           var getModeDefinition = bungie.GetActivityModeDefinitionAsync(pgcr.Pgcr.ActivityDetails.Mode);
                                           var activity = new DestinyUserActivity(pgcr.Pgcr, await getActivityDefinition, await getModeDefinition,
                                                PlayerData.CharacterIDs[i]);
                                           var msGraph = new MsGraphService(Shared.SharedData.MsGraphClientId);
                                           await msGraph.CreateOrReplaceActivityAsync(activity.Activity);
                                       }));
                }
                IsSyncIdle = true;
            }
        }

        public async Task ToggleBackgroundTask()
        {
            IsRegisterIdle = false;
            var taskName = "RefreshActivitiesBackgroundTask";
            if (isBackgroundTaskRegistered(taskName))
            {
                unregisterBackgroundTask(taskName);
                IsTaskRegistered = false;
            }
            else
            {
                await SyncToTimeline();
                await registerBackgroundTask(taskName);
                IsTaskRegistered = true;
            }
            IsRegisterIdle = true;
        }

        private bool isBackgroundTaskRegistered(string taskName)
        {
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == taskName)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task registerBackgroundTask(string taskName)
        {
            var requestStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (requestStatus != BackgroundAccessStatus.DeniedBySystemPolicy &&
                requestStatus != BackgroundAccessStatus.DeniedByUser)
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                var basePlayerData = new DestinyPlayerInformation
                {
                    CharacterIDs = PlayerData.CharacterIDs,
                    MembershipID = PlayerData.MembershipID,
                    MembershipType = PlayerData.MembershipType
                };
                var json = JsonConvert.SerializeObject(basePlayerData);
                localSettings.Values["player"] = json;
                
                var builder = new BackgroundTaskBuilder
                {
                    Name = taskName,
                    TaskEntryPoint = "Destiny2PgcrTimelineBackgroundTasks.RefreshActivitiesBackgroundTask"
                };
                builder.SetTrigger(new TimeTrigger(30, false));
                BackgroundTaskRegistration task = builder.Register();
            }
        }

        private void unregisterBackgroundTask(string taskName)
        {
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == taskName)
                {
                    task.Value.Unregister(true);
                    break;
                }
            }
        }
    }
}
