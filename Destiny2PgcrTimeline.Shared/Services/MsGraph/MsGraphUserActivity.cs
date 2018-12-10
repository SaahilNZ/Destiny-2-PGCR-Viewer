using AdaptiveCards;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destiny2PgcrTimeline.Shared.Services.MsGraph
{
    public class MsGraphUserActivity
    {
        public string AppActivityId;
        public string ActivitySourceHost;
        public string AppDisplayName;
        public string ActivationUrl;
        public string ContentUrl;
        public string FallbackUrl;
        public MsGraphVisualElements VisualElements;
        public List<MsGraphHistoryItem> HistoryItems;
    }

    public class MsGraphVisualElements
    {
        public MsGraphAttribution Attribution;
        public string Description;
        public string BackgroundColor;
        public string DisplayText;
        public AdaptiveCard Content;
    }

    public class MsGraphAttribution
    {
        public string IconUrl;
        public string AlternateText;
        public string AddImageQuery;
    }

    public class MsGraphHistoryItem
    {
        public DateTime StartedDateTime;
        public DateTime LastActiveDateTime;
    }
}
