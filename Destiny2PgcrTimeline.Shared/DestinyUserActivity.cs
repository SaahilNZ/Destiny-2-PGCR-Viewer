using AdaptiveCards;
using Destiny2PgcrTimeline.Shared.Services.Bungie;
using Destiny2PgcrTimeline.Shared.Services.Bungie.Definitions;
using Destiny2PgcrTimeline.Shared.Services.MsGraph;
using System;
using System.Collections.Generic;

namespace Destiny2PgcrTimeline.Shared
{
    public class DestinyUserActivity
    {
        public MsGraphUserActivity Activity { get; private set; }

        public DestinyUserActivity(DestinyActivity destinyActivity, DestinyActivityDefinition definition,
            DestinyActivityModeDefinition modeDefinition, string characterId)
        {
            /*
             * Download and unpack world database
             * /common/destiny2_content/sqlite/en/world_sql_content_7227d0c5fd50443c8dd9e2e35f353f69.content
             * mode maps to modeType in DestinyActivityModeDefinition (game type)
             * referenceId maps to hash in DestinyActivityDefinition (map)
             * directoryActivityHash maps to hash in DestinyActivityDefinition (playlist)
             * 
             * activity definitions
             * https://bungie-net.github.io/multi/schema_Destiny-HistoricalStats-Definitions-DestinyActivityModeType.html#schema_Destiny-HistoricalStats-Definitions-DestinyActivityModeType
             */

            var activityId = destinyActivity.ActivityDetails.InstanceId;
            var contentUrl = $"https://www.bungie.net/en/PGCR/{activityId}?character={characterId}";
            var gameTypeIconUrl = $"https://www.bungie.net{modeDefinition.DisplayProperties.Icon}";
            var gameType = modeDefinition.DisplayProperties.Name;
            var map = definition.DisplayProperties.Name;
            var startTime = destinyActivity.Period;
            var endTime = destinyActivity.Period.AddSeconds(destinyActivity.Values["activityDurationSeconds"].Basic.Value);
            var mapImageUrl = $"https://www.bungie.net{definition.PgcrImage}";
            var placement = destinyActivity.Values["standing"].Basic.DisplayValue;
            var placementColour = placement == "Victory" || placement == "1" ? AdaptiveTextColor.Good : AdaptiveTextColor.Attention;
            var stat1Name = "Efficiency";
            var stat1Value = destinyActivity.Values["efficiency"].Basic.DisplayValue;
            var stat2Name = "Opponents Defeated";
            var stat2Value = destinyActivity.Values["opponentsDefeated"].Basic.DisplayValue;
            Activity = BuildActivity(activityId, contentUrl, gameTypeIconUrl, gameType, map, startTime,
                endTime, mapImageUrl, placementColour, placement, stat1Name, stat1Value, stat2Name,
                stat2Value);
        }

        private MsGraphUserActivity BuildActivity(string activityId, string contentUrl,
            string gameTypeIconUrl, string gameType, string map, DateTime startTime,
            DateTime endTime, string mapImageUrl, AdaptiveTextColor placementColour,
            string placement, string stat1Name, string stat1Value, string stat2Name, string stat2Value)
        {
            return new MsGraphUserActivity
            {
                AppActivityId = $"/destiny2-pgcr?{activityId}",
                ActivitySourceHost = "destiny2-activity-tracker",
                AppDisplayName = "Destiny 2",
                ActivationUrl = contentUrl,
                ContentUrl = contentUrl,
                FallbackUrl = contentUrl,
                VisualElements = new MsGraphVisualElements
                {
                    Attribution = new MsGraphAttribution
                    {
                        IconUrl = gameTypeIconUrl,
                        AlternateText = $"{gameType} // {map}",
                        AddImageQuery = "false"
                    },
                    Description = $"{gameType} on {map}",
                    BackgroundColor = "#000000",
                    DisplayText = "Destiny 2",
                    Content = BuildCard(mapImageUrl, gameTypeIconUrl, map, gameType, placementColour,
                                    placement, stat1Name, stat1Value, stat2Name, stat2Value)
                },
                HistoryItems = new List<MsGraphHistoryItem>
                {
                    new MsGraphHistoryItem
                    {
                        StartedDateTime = startTime,
                        LastActiveDateTime = endTime
                    }
                }
            };
        }

        private AdaptiveCard BuildCard(string mapImageUrl, string gameTypeIconUrl,
            string map, string gameType, AdaptiveTextColor placementColour, string placement,
            string stat1Name, string stat1Value, string stat2Name, string stat2Value)
        {
            var newCard = new AdaptiveCard();
            newCard.BackgroundImage = new Uri(mapImageUrl);
            newCard.Body.Add(new AdaptiveContainer()
            {
                Items =
                {
                    new AdaptiveColumnSet()
                    {
                        Height = AdaptiveHeight.Stretch,
                        Spacing = AdaptiveSpacing.None,
                        Columns =
                        {
                            new AdaptiveColumn()
                            {
                                Spacing = AdaptiveSpacing.None,
                                Height = AdaptiveHeight.Stretch,
                                VerticalContentAlignment = AdaptiveVerticalContentAlignment.Center,
                                Items =
                                {
                                    new AdaptiveImage()
                                    {
                                        Spacing = AdaptiveSpacing.None,
                                        Url = new Uri(gameTypeIconUrl),
                                        Size = AdaptiveImageSize.Small
                                    }
                                },
                                Width = "auto"
                            },
                            new AdaptiveColumn()
                            {
                                Spacing = AdaptiveSpacing.Small,
                                VerticalContentAlignment = AdaptiveVerticalContentAlignment.Center,
                                Items =
                                {
                                    new AdaptiveTextBlock()
                                    {
                                        Spacing = AdaptiveSpacing.None,
                                        Size = AdaptiveTextSize.ExtraLarge,
                                        Weight = AdaptiveTextWeight.Bolder,
                                        Text = map,
                                        Wrap = true,
                                        MaxLines = 2
                                    },
                                    new AdaptiveColumnSet()
                                    {
                                        Spacing = AdaptiveSpacing.None,
                                        Columns =
                                        {
                                            new AdaptiveColumn()
                                            {
                                                Spacing = AdaptiveSpacing.None,
                                                Items =
                                                {
                                                    new AdaptiveTextBlock()
                                                    {
                                                        Spacing = AdaptiveSpacing.None,
                                                        Text = gameType
                                                    }
                                                },
                                                Width = "stretch"
                                            },
                                            new AdaptiveColumn()
                                            {
                                                Items =
                                                {
                                                    new AdaptiveTextBlock()
                                                    {
                                                        HorizontalAlignment = AdaptiveHorizontalAlignment.Right,
                                                        Weight = AdaptiveTextWeight.Bolder,
                                                        Color = placementColour,
                                                        Text = placement
                                                    }
                                                },
                                                Width = "auto"
                                            }
                                        }
                                    }
                                },
                                Width = "stretch"
                            }
                        }
                    },
                    new AdaptiveColumnSet()
                    {
                        Spacing = AdaptiveSpacing.Medium,
                        Height = AdaptiveHeight.Stretch,
                        Columns =
                        {
                            new AdaptiveColumn()
                            {
                                Items =
                                {
                                    new AdaptiveTextBlock()
                                    {
                                        Size = AdaptiveTextSize.Small,
                                        Text = stat1Name
                                    },
                                    new AdaptiveTextBlock()
                                    {
                                        Spacing = AdaptiveSpacing.None,
                                        Size = AdaptiveTextSize.Large,
                                        Weight = AdaptiveTextWeight.Bolder,
                                        Text = stat1Value
                                    }
                                },
                                Width = "stretch"
                            },
                            new AdaptiveColumn()
                            {
                                Items =
                                {
                                    new AdaptiveTextBlock()
                                    {
                                        Size = AdaptiveTextSize.Small,
                                        Text = stat2Name
                                    },
                                    new AdaptiveTextBlock()
                                    {
                                        Spacing = AdaptiveSpacing.None,
                                        Size = AdaptiveTextSize.Large,
                                        Weight = AdaptiveTextWeight.Bolder,
                                        Text = stat2Value
                                    }
                                },
                                Width = "stretch"
                            }
                        }
                    }
                }
            });
            return newCard;
        }
    }
}