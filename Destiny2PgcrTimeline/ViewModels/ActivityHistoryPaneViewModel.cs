using AdaptiveCards.Rendering.Uwp;
using Destiny2PgcrTimeline.Shared.Services.Bungie;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Destiny2PgcrTimeline.ViewModels
{
    internal class ActivityHistoryPaneViewModel : ViewModelBase
    {
        private ObservableCollection<PgcrCardViewModel> char1ActivityHistory;
        private ObservableCollection<PgcrCardViewModel> char2ActivityHistory;
        private ObservableCollection<PgcrCardViewModel> char3ActivityHistory;
        private CharacterNameplateViewModel character1;
        private CharacterNameplateViewModel character2;
        private CharacterNameplateViewModel character3;
        private bool showAllCharacters;
        private int selectedCharacter;

        public delegate void NameplateTappedEventHandler(object sender);
        public event NameplateTappedEventHandler NameplateTapped;

        public ObservableCollection<PgcrCardViewModel> Char1ActivityHistory
        {
            get { return char1ActivityHistory; }
            set
            {
                char1ActivityHistory = value;
                NotifyPropertyChanged(nameof(Char1ActivityHistory));
            }
        }

        public ObservableCollection<PgcrCardViewModel> Char2ActivityHistory
        {
            get { return char2ActivityHistory; }
            set
            {
                char2ActivityHistory = value;
                NotifyPropertyChanged(nameof(Char2ActivityHistory));
            }
        }

        public ObservableCollection<PgcrCardViewModel> Char3ActivityHistory
        {
            get { return char3ActivityHistory; }
            set
            {
                char3ActivityHistory = value;
                NotifyPropertyChanged(nameof(Char3ActivityHistory));
            }
        }

        public CharacterNameplateViewModel Character1
        {
            get { return character1; }
            set
            {
                character1 = value;
                NotifyPropertyChanged(nameof(Character1));
            }
        }

        public CharacterNameplateViewModel Character2
        {
            get { return character2; }
            set
            {
                character2 = value;
                NotifyPropertyChanged(nameof(Character2));
            }
        }

        public CharacterNameplateViewModel Character3
        {
            get { return character3; }
            set
            {
                character3 = value;
                NotifyPropertyChanged(nameof(Character3));
            }
        }

        public bool ShowAllCharacters
        {
            get { return showAllCharacters; }
            set
            {
                showAllCharacters = value;
                NotifyPropertyChanged(nameof(ShowAllCharacters));
                NotifyPropertyChanged(nameof(ShowCharacter1));
                NotifyPropertyChanged(nameof(ShowCharacter2));
                NotifyPropertyChanged(nameof(ShowCharacter3));
            }
        }

        public int SelectedCharacter
        {
            get { return selectedCharacter; }
            set
            {
                selectedCharacter = value;
                NotifyPropertyChanged(nameof(SelectedCharacter));
                NotifyPropertyChanged(nameof(ShowCharacter1));
                NotifyPropertyChanged(nameof(ShowCharacter2));
                NotifyPropertyChanged(nameof(ShowCharacter3));
            }
        }

        public bool ShowCharacter1 => ShowAllCharacters || SelectedCharacter == 1;
        public bool ShowCharacter2 => ShowAllCharacters || SelectedCharacter == 2;
        public bool ShowCharacter3 => ShowAllCharacters || SelectedCharacter == 3;

        public ActivityHistoryPaneViewModel()
        {
            Char1ActivityHistory = new ObservableCollection<PgcrCardViewModel>();
            Char2ActivityHistory = new ObservableCollection<PgcrCardViewModel>();
            Char3ActivityHistory = new ObservableCollection<PgcrCardViewModel>();
            Character1 = new CharacterNameplateViewModel
            {
                ElementVisibility = Visibility.Collapsed
            };
            Character2 = new CharacterNameplateViewModel
            {
                ElementVisibility = Visibility.Collapsed
            };
            Character3 = new CharacterNameplateViewModel
            {
                ElementVisibility = Visibility.Collapsed
            };
        }

        public void PopulateActivityHistory(DestinyPlayerData playerData)
        {
            var activitySources = new List<ObservableCollection<PgcrCardViewModel>>
            {
                Char1ActivityHistory, Char2ActivityHistory, Char3ActivityHistory
            };
            var nameplates = new List<CharacterNameplateViewModel>
            {
                Character1, Character2, Character3
            };
            
            SelectedCharacter = 1;

            var adaptiveCardRenderer = new AdaptiveCardRenderer
            {
                HostConfig = new AdaptiveHostConfig
                {
                    FontFamily = "Segoe UI",
                    FontSizes = new AdaptiveFontSizesConfig
                    {
                        Small = 12,
                        Default = 14,
                        Large = 20,
                        ExtraLarge = 24
                    },
                    FontWeights = new AdaptiveFontWeightsConfig
                    {
                        Lighter = 200,
                        Default = 400,
                        Bolder = 700
                    },
                    ContainerStyles = new AdaptiveContainerStylesDefinition
                    {
                        Default = new AdaptiveContainerStyleDefinition
                        {
                            BackgroundColor = Color.FromArgb(255, 83, 84, 84),
                            ForegroundColors = new AdaptiveColorsConfig
                            {
                                Default = new AdaptiveColorConfig
                                {
                                    Default = Color.FromArgb(255, 255, 255, 255),
                                    Subtle = Color.FromArgb(255, 156, 158, 159)
                                },
                                Attention = new AdaptiveColorConfig
                                {
                                    Default = Color.FromArgb(255, 255, 0, 0),
                                    Subtle = Color.FromArgb(221, 255, 0, 0)
                                },
                                Good = new AdaptiveColorConfig
                                {
                                    Default = Color.FromArgb(255, 0, 255, 0),
                                    Subtle = Color.FromArgb(221, 0, 255, 0)
                                }
                            }
                        }
                    },
                    ImageSizes = new AdaptiveImageSizesConfig
                    {
                        Small = 40,
                        Medium = 80,
                        Large = 120
                    },
                    ImageSet = new AdaptiveImageSetConfig
                    {
                        ImageSize = ImageSize.Medium,
                        MaxImageHeight = 100
                    }
                }
            };

            foreach (var activitySource in activitySources)
            {
                activitySource.Clear();
            }
            foreach (var nameplate in nameplates)
            {
                nameplate.ClearProperties();
            }

            for (int i = 0; i < playerData.CharacterNameplates.Count; i++)
            {
                nameplates[i].ElementVisibility = playerData.CharacterNameplates[i].ElementVisibility;
                nameplates[i].ClassName = playerData.CharacterNameplates[i].ClassName;
                nameplates[i].Race = playerData.CharacterNameplates[i].Race;
                nameplates[i].Gender = playerData.CharacterNameplates[i].Gender;
                nameplates[i].Level = playerData.CharacterNameplates[i].Level;
                nameplates[i].Power = playerData.CharacterNameplates[i].Power;
                nameplates[i].Emblem = playerData.CharacterNameplates[i].Emblem;

                if (playerData.ActivityHistoryLists[i].Count == 0)
                {
                    activitySources[i].Add(null);
                }
                else
                {
                    foreach (var pgcr in playerData.ActivityHistoryLists[i])
                    {
                        activitySources[i].Add(pgcr);
                    }
                }
            }
        }

        public void OnCharacterSelected()
        {
            NameplateTapped?.Invoke(this);
        }
    }
}
