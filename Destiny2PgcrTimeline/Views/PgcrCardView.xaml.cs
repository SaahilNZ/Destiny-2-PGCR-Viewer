using AdaptiveCards.Rendering.Uwp;
using Destiny2PgcrTimeline.Shared.Services.Bungie;
using Destiny2PgcrTimeline.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Destiny2PgcrTimeline.Views
{
    internal sealed partial class PgcrCardView : UserControl
    {
        public DestinyActivity Pgcr
        {
            get { return (DestinyActivity)GetValue(PgcrProperty); }
            set { SetValue(PgcrProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pgcr.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PgcrProperty =
            DependencyProperty.Register("Pgcr", typeof(DestinyActivity), typeof(PgcrCardView), new PropertyMetadata(null));
        
        public string CharacterId
        {
            get { return (string)GetValue(CharacterIdProperty); }
            set { SetValue(CharacterIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CharacterId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CharacterIdProperty =
            DependencyProperty.Register("CharacterId", typeof(string), typeof(PgcrCardView), new PropertyMetadata(null));
        
        public PgcrCardView()
        {
            this.InitializeComponent();
        }

        private async void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            var bungie = new BungieService(Shared.SharedData.BungieApiKey);

            var adaptiveCardRenderer = new AdaptiveCardRenderer
            {
                HostConfig = new AdaptiveHostConfig
                {
                    FontFamily = "Segoe UI",
                    FontSizes = new AdaptiveFontSizesConfig
                    {
                        Small = 12,
                        Default = 14,
                        Large = 20,
                        ExtraLarge = 24
                    },
                    FontWeights = new AdaptiveFontWeightsConfig
                    {
                        Lighter = 200,
                        Default = 400,
                        Bolder = 700
                    },
                    ContainerStyles = new AdaptiveContainerStylesDefinition
                    {
                        Default = new AdaptiveContainerStyleDefinition
                        {
                            BackgroundColor = Color.FromArgb(255, 83, 84, 84),
                            ForegroundColors = new AdaptiveColorsConfig
                            {
                                Default = new AdaptiveColorConfig
                                {
                                    Default = Color.FromArgb(255, 255, 255, 255),
                                    Subtle = Color.FromArgb(255, 156, 158, 159)
                                },
                                Attention = new AdaptiveColorConfig
                                {
                                    Default = Color.FromArgb(255, 255, 0, 0),
                                    Subtle = Color.FromArgb(221, 255, 0, 0)
                                },
                                Good = new AdaptiveColorConfig
                                {
                                    Default = Color.FromArgb(255, 0, 255, 0),
                                    Subtle = Color.FromArgb(221, 0, 255, 0)
                                }
                            }
                        }
                    },
                    ImageSizes = new AdaptiveImageSizesConfig
                    {
                        Small = 40,
                        Medium = 80,
                        Large = 120
                    },
                    ImageSet = new AdaptiveImageSetConfig
                    {
                        ImageSize = ImageSize.Medium,
                        MaxImageHeight = 100
                    }
                }
            };

            var getActivityDefinition = bungie.GetActivityDefinitionAsync(Pgcr.ActivityDetails.ReferenceId);
            var getModeDefinition = bungie.GetActivityModeDefinitionAsync(Pgcr.ActivityDetails.Mode);

            var activity = new DestinyUserActivity(Pgcr, await getActivityDefinition, await getModeDefinition, CharacterId);
            var renderedCard = adaptiveCardRenderer.RenderAdaptiveCardFromJsonString(activity.Activity.VisualElements.Content.ToJson());
            if (renderedCard.FrameworkElement != null)
            {
                Grid uiCard = renderedCard.FrameworkElement as Grid;
                uiCard.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                var background = uiCard.Children.OfType<Image>().FirstOrDefault();
                if (background != null)
                {
                    background.Opacity = 0.5;
                }
                uiCard.Width = 316;
                uiCard.Height = 174;
                uiCard.Margin = new Thickness(0, 0, 0, 24);
                LayoutRoot.Child = uiCard;
            }
        }
    }
}
