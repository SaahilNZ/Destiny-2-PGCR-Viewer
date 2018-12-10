using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destiny2PgcrTimeline.Shared.Services.Bungie.Definitions
{
    public abstract class DestinyDefinition
    {
        public DefinitionDisplayProperties DisplayProperties;
    }

    public class DefinitionDisplayProperties
    {
        public string Name;
        public string Icon;
    }
}
