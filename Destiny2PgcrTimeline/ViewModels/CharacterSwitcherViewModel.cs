using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destiny2PgcrTimeline.ViewModels
{
    internal class CharacterSwitcherViewModel : ViewModelBase
    {
        private ObservableCollection<CharacterNameplateViewModel> characters;
        private bool isVisible;

        public delegate void CharacterSelectedEventHandler(object sender, int characterIndex);
        public event CharacterSelectedEventHandler CharacterSelected;

        public delegate void SearchPlayerClickedEventHandler(object sender);
        public event SearchPlayerClickedEventHandler SearchPlayerClicked;

        public delegate void SettingsClickedEventHandler(object sender);
        public event SettingsClickedEventHandler SettingsClicked;

        public ObservableCollection<CharacterNameplateViewModel> Characters
        {
            get { return characters; }
            set
            {
                characters = value;
                NotifyPropertyChanged(nameof(Characters));
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

        public CharacterSwitcherViewModel()
        {
            Characters = new ObservableCollection<CharacterNameplateViewModel>();
        }

        public void OnCharacterSelected(int characterIndex)
        {
            CharacterSelected?.Invoke(this, characterIndex);
            IsVisible = false;
        }

        public void OnSearchPlayerClicked()
        {
            SearchPlayerClicked?.Invoke(this);
            IsVisible = false;
        }

        public void OnSettingsClicked()
        {
            SettingsClicked?.Invoke(this);
            IsVisible = false;
        }

        public void OnBackgroundClicked()
        {
            IsVisible = false;
        }

        public void PopulateNameplates(DestinyPlayerData data)
        {
            Characters.Clear();
            foreach (var nameplate in data.CharacterNameplates)
            {
                Characters.Add(nameplate);
            }
        }
    }
}
