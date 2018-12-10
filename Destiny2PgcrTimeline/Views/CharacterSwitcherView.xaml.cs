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
    internal sealed partial class CharacterSwitcherView : UserControl
    {
        public CharacterSwitcherViewModel ViewModel
        {
            get { return (CharacterSwitcherViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(CharacterSwitcherViewModel), typeof(CharacterSwitcherView), new PropertyMetadata(null));

        public bool IsNarrowMode
        {
            get { return (bool)GetValue(IsNarrowModeProperty); }
            set { SetValue(IsNarrowModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsNarrowMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNarrowModeProperty =
            DependencyProperty.Register("IsNarrowMode", typeof(bool), typeof(CharacterSwitcherView), new PropertyMetadata(true));

        public CharacterSwitcherView()
        {
            this.InitializeComponent();
        }

        private void OnCharacterSelected(object sender, ItemClickEventArgs e)
        {
            ViewModel.OnCharacterSelected(ViewModel.Characters.IndexOf(e.ClickedItem as CharacterNameplateViewModel) + 1);
        }
    }
}
