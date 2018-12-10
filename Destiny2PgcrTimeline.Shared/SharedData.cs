using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destiny2PgcrTimeline.Shared
{
    public class SharedData
    {
        public enum BungieMembershipType
        {
            Xbox = 1,
            PlayStation = 2,
            PC = 4
        };

        public static readonly string BungieApiKey = Secrets.BUNGIE_API_KEY;
        public static readonly string MsGraphClientId = Secrets.MS_GRAPH_CLIENT_ID;
    }
}
