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
    internal sealed partial class ActivityHistoryPaneView : UserControl
    {
        public ActivityHistoryPaneViewModel ViewModel
        {
            get { return (ActivityHistoryPaneViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ActivityHistoryPaneViewModel), typeof(ActivityHistoryPaneView), new PropertyMetadata(null));
        
        public bool IsNarrowMode
        {
            get { return (bool)GetValue(IsNarrowModeProperty); }
            set { SetValue(IsNarrowModeProperty, value); }
        }
        
        public static readonly DependencyProperty IsNarrowModeProperty =
            DependencyProperty.Register("IsNarrowMode", typeof(bool), typeof(ActivityHistoryPaneView),
                new PropertyMetadata(true, new PropertyChangedCallback(OnNarrowModeChanged)));

        public ActivityHistoryPaneView()
        {
            this.InitializeComponent();
        }

        private static void OnNarrowModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var view = obj as ActivityHistoryPaneView;
            if (view.ViewModel != null)
            {
                view.ViewModel.ShowAllCharacters = !(bool)args.NewValue;
            }
        }

        private void Nameplate_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsNarrowMode)
            {
                ViewModel.OnCharacterSelected();
            }
        }
    }
}
