using Destiny2PgcrTimeline.Shared.Services.Bungie;
using Destiny2PgcrTimeline.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    internal sealed partial class PlayerSearchPaneView : UserControl
    {
        public PlayerSearchPaneViewModel ViewModel
        {
            get { return (PlayerSearchPaneViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PlayerSearchPaneViewModel),
                typeof(PlayerSearchPaneView), new PropertyMetadata(null));
        

        public PlayerSearchPaneView()
        {
            this.InitializeComponent();
        }

        private void OnPlayerSelected(object sender, ItemClickEventArgs e)
        {
            ViewModel.OnPlayerSelected((e.ClickedItem as PlayerSearchResultViewModel).Player);
        }
    }
}
