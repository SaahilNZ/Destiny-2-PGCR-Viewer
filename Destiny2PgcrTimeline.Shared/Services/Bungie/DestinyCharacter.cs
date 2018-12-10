using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destiny2PgcrTimeline.Shared.Services.Bungie
{
    public class DestinyCharacterResponse
    {
        public DestinyCharacter Character;
    }

    public class DestinyCharacter
    {
        public DestinyCharacterData Data;
    }

    public class DestinyCharacterData
    {
        public int Light;
        public uint RaceHash;
        public uint GenderHash;
        public uint ClassHash;
        public string EmblemBackgroundPath;
        public int BaseCharacterLevel;
    }
}
