using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destiny2PgcrTimeline.Shared.Services.Bungie
{
    public class DestinyActivityHistoryResponse
    {
        public DestinyActivity[] Activities;
    }

    public class DestinyActivity
    {
        public DateTime Period;
        public DestinyActivityDetails ActivityDetails;
        public Dictionary<string, ActivityStat> Values;
    }

    public class DestinyActivityDetails
    {
        public uint ReferenceId;
        public string InstanceId;
        public int Mode;
    }

    public class ActivityStat
    {
        public string StatId;
        public ActivityStatValue Basic;
    }

    public class ActivityStatValue
    {
        public double Value;
        public string DisplayValue;
    }
}
