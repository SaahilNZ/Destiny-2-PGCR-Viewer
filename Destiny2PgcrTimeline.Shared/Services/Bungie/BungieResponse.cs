using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destiny2PgcrTimeline.Shared.Services.Bungie
{
    public class BungieResponse<T>
    {
        public T Response;
        public int ErrorCode;
        public int ThrottleSeconds;
        public string Message;
    }
}
