using Destiny2PgcrTimeline.Shared.Services.Bungie;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destiny2PgcrTimeline.ViewModels
{
    internal class PlayerSearchPaneViewModel : ViewModelBase
    {
        private string username;
        private ObservableCollection<PlayerSearchResultViewModel> searchResults;
        private bool isNarrowVisible;

        public delegate void PlayerSelectedEventHandler(object sender, DestinyPlayer player);
        public event PlayerSelectedEventHandler PlayerSelected;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                NotifyPropertyChanged(nameof(Username));
            }
        }

        public ObservableCollection<PlayerSearchResultViewModel> SearchResults
        {
            get { return searchResults; }
            set
            {
                searchResults = value;
                NotifyPropertyChanged(nameof(SearchResults));
            }
        }

        public bool IsNarrowVisible
        {
            get { return isNarrowVisible; }
            set
            {
                isNarrowVisible = value;
                NotifyPropertyChanged(nameof(IsNarrowVisible));
            }
        }


        public PlayerSearchPaneViewModel()
        {
            SearchResults = new ObservableCollection<PlayerSearchResultViewModel>();
        }

        public async void SearchAsync()
        {
            SearchResults.Clear();
            if (!string.IsNullOrEmpty(Username))
            {
                var bungie = new BungieService(Shared.SharedData.BungieApiKey);

                var destinyPlayers = await bungie.GetDestinyPlayers(-1, Username);
                foreach (var player in destinyPlayers)
                {
                    SearchResults.Add(new PlayerSearchResultViewModel(player));
                }
            }
        }

        public void OnPlayerSelected(DestinyPlayer player)
        {
            PlayerSelected?.Invoke(this, player);
        }
    }
}
