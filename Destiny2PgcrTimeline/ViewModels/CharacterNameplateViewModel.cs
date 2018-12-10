using Destiny2PgcrTimeline.Shared.Services.Bungie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Destiny2PgcrTimeline.ViewModels
{
    internal class CharacterNameplateViewModel : ViewModelBase
    {
        private string className;
        private string race;
        private string gender;
        private int level;
        private int power;
        private ImageBrush emblem;
        private Visibility elementVisibility;

        public string ClassName
        {
            get { return className; }
            set
            {
                className = value;
                NotifyPropertyChanged(nameof(ClassName));
            }
        }

        public string Race
        {
            get { return race; }
            set
            {
                race = value;
                NotifyPropertyChanged(nameof(Race));
            }
        }

        public string Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                NotifyPropertyChanged(nameof(Gender));
            }
        }

        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                NotifyPropertyChanged(nameof(Level));
            }
        }

        public int Power
        {
            get { return power; }
            set
            {
                power = value;
                NotifyPropertyChanged(nameof(Power));
            }
        }

        public ImageBrush Emblem
        {
            get { return emblem; }
            set
            {
                emblem = value;
                NotifyPropertyChanged(nameof(Emblem));
            }
        }
        
        public Visibility ElementVisibility
        {
            get { return elementVisibility; }
            set
            {
                elementVisibility = value;
                NotifyPropertyChanged(nameof(ElementVisibility));
            }
        }

        internal void ClearProperties()
        {
            ClassName = "";
            Race = "";
            Gender = "";
            Level = 0;
            Power = 0;
            Emblem = null;
            ElementVisibility = Visibility.Collapsed;
        }
    }
}
