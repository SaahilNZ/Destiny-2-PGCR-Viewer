using Destiny2PgcrTimeline.Shared.Services.Bungie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destiny2PgcrTimeline.ViewModels
{
    internal class PgcrCardViewModel : ViewModelBase
    {
        private DestinyActivity pgcr;

        public DestinyActivity Pgcr
        {
            get { return pgcr; }
            set
            {
                pgcr = value;
                NotifyPropertyChanged(nameof(Pgcr));
            }
        }

        private string characterId;

        public string CharacterId
        {
            get { return characterId; }
            set
            {
                characterId = value;
                NotifyPropertyChanged(nameof(CharacterId));
            }
        }
    }
}
