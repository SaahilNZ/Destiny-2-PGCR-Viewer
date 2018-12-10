using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destiny2PgcrTimeline.Shared.Services.Bungie
{
    public class DestinyProfileResponse
    {
        public DestinyProfile Profile;
    }

    public class DestinyProfile
    {
        public DestinyProfileData Data;
    }

    public class DestinyProfileData
    {
        public List<string> CharacterIds;
    }
}
