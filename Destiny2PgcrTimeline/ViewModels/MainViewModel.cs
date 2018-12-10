using Destiny2PgcrTimeline.Shared.Services.Bungie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destiny2PgcrTimeline.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        public PlayerSearchPaneViewModel SearchPane { get; private set; }
        public ActivityHistoryPaneViewModel ActivityHistoryPane { get; private set; }
        public CharacterSwitcherViewModel CharacterSwitcher { get; private set; }
        public SettingsDialogViewModel SettingsDialog { get; private set; }

        public MainViewModel(ActivityHistoryPaneViewModel activityHistoryPane)
        {
            SearchPane = new PlayerSearchPaneViewModel();
            ActivityHistoryPane = activityHistoryPane;
            CharacterSwitcher = new CharacterSwitcherViewModel();
            SettingsDialog = new SettingsDialogViewModel();
            SearchPane.PlayerSelected += OnPlayerSelected;
            CharacterSwitcher.CharacterSelected += OnCharacterSelected;
            CharacterSwitcher.SearchPlayerClicked += OnSearchPlayerNarrowClicked;
            ActivityHistoryPane.NameplateTapped += OnNameplateTapped;
            CharacterSwitcher.SettingsClicked += OnSettingsClicked;
            SearchPane.IsNarrowVisible = true;
        }

        public void OpenSettings()
        {
            SettingsDialog.IsVisible = true;
        }

        private async void OnPlayerSelected(object sender, DestinyPlayer player)
        {
            var data = await GetPlayerDataAsync(player);
            ActivityHistoryPane.PopulateActivityHistory(data);
            CharacterSwitcher.PopulateNameplates(data);
            SettingsDialog.PlayerData = data;
            SearchPane.IsNarrowVisible = false;
        }

        private void OnCharacterSelected(object sender, int characterIndex)
        {
            ActivityHistoryPane.SelectedCharacter = characterIndex;
        }

        private void OnSearchPlayerNarrowClicked(object sender)
        {
            SearchPane.IsNarrowVisible = true;
        }

        private void OnNameplateTapped(object sender)
        {
            CharacterSwitcher.IsVisible = true;
        }

        private void OnSettingsClicked(object sender)
        {
            SettingsDialog.IsVisible = true;
        }
        private async Task<DestinyPlayerData> GetPlayerDataAsync(DestinyPlayer player)
        {
            var appService = new AppService();
            return await appService.GetPlayerDataAsync(player);
        }
    }
}
