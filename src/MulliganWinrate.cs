using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Graveyard;
using HDT.Plugins.Graveyard;
using Hearthstone_Deck_Tracker.API;
using Core = Hearthstone_Deck_Tracker.API.Core;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;

namespace MulliganWinrate
{
    public class MulliganWinrate
    {
        // The views

        public MulliganView Mulligan;
        private StackPanel _friendlyPanel;

        public static InputManager Input;

        public MulliganWinrate()
        {
            // Create container
            _friendlyPanel = new StackPanel();
            _friendlyPanel.Orientation = Orientation.Vertical;
            Core.OverlayCanvas.Children.Add(_friendlyPanel);
            Canvas.SetTop(_friendlyPanel, Settings.Default.PlayerTop);
            Canvas.SetLeft(_friendlyPanel, Settings.Default.PlayerLeft);

            Input = new InputManager(_friendlyPanel);

            Settings.Default.PropertyChanged += SettingsChanged;
            SettingsChanged(null, null);

            // Connect events
            GameEvents.OnGameStart.Add(Reset);
            GameEvents.OnPlayerDraw.Add(AddToMulligan);
            GameEvents.OnGameEnd.Add(Reset);
            DeckManagerEvents.OnDeckSelected.Add(d => Reset());

        }

        private void AddToMulligan(Card card)
        {
            if (!Core.Game.IsMulliganDone)
            {
                Mulligan.Update(card);
            }
            else
            {
                Reset();
            }
        }

        //on year change clear out the grid and update the data
        private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _friendlyPanel.RenderTransform = new ScaleTransform(Settings.Default.FriendlyScale / 100,
                Settings.Default.FriendlyScale / 100);
            _friendlyPanel.Opacity = Settings.Default.FriendlyOpacity / 100;
        }

        public void Dispose()
        {
            Core.OverlayCanvas.Children.Remove(_friendlyPanel);

            Input.Dispose();
        }

        /**
        * Clear then recreate all Views.
        */
        public void Reset()
        {
            _friendlyPanel.Children.Clear();
            
            
            Mulligan = new MulliganView();
            Mulligan.Label.Visibility = Visibility.Hidden;
            _friendlyPanel.Children.Add(Mulligan);
        }
    }
}