using Destiny2PgcrTimeline.Shared.Services.Bungie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Destiny2PgcrTimeline.ViewModels
{
    internal class PlayerSearchResultViewModel : ViewModelBase
    {
        private string username;
        private Color platformColor;
        private ImageSource platformIcon;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                NotifyPropertyChanged(nameof(Username));
            }
        }
        
        public Color PlatformColor
        {
            get { return platformColor; }
            set
            {
                platformColor = value;
                NotifyPropertyChanged(nameof(PlatformColor));
            }
        }
        
        public ImageSource PlatformIcon
        {
            get { return platformIcon; }
            set
            {
                platformIcon = value;
                NotifyPropertyChanged(nameof(PlatformIcon));
            }
        }
        public DestinyPlayer Player { get; private set; }

        public PlayerSearchResultViewModel(DestinyPlayer player)
        {
            Player = player;
            Username = player.DisplayName;
            switch (player.MembershipType)
            {
                case 1:
                    PlatformColor = Color.FromArgb(255, 16, 124, 16);
                    PlatformIcon = new BitmapImage(new Uri("ms-appx:///Resources/Xbox.png"));
                    break;
                case 2:
                    PlatformColor = Color.FromArgb(255, 0, 55, 145);
                    PlatformIcon = new BitmapImage(new Uri("ms-appx:///Resources/PS4.png"));
                    break;
                case 4:
                    PlatformColor = Color.FromArgb(255, 3, 135, 209);
                    PlatformIcon = new BitmapImage(new Uri("ms-appx:///Resources/PC.png"));
                    break;
                default:
                    PlatformColor = Color.FromArgb(255, 128, 128, 128);
                    PlatformIcon = new BitmapImage(new Uri("ms-appx:///Resources/Unknown.png"));
                    break;
            }
        }
    }
}
