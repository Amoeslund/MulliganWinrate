using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using HDT.Plugins.Graveyard;
using HearthDb;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.HsReplay;
using Hearthstone_Deck_Tracker.HsReplay.Utility;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Newtonsoft.Json;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;
using Orientation = System.Windows.Controls.Orientation;

// ReSharper disable InconsistentNaming

namespace MulliganWinrate
{
    public class MulliganWinrate
    {
        // The views

        public MulliganView Mulligan;
        private StackPanel _friendlyPanel;
        

        public static InputManager Input;
        private Dictionary<int, double> _winrates;
        private static double _deckWinrate;

        public MulliganWinrate()
        {
            // Create container
            _friendlyPanel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            CoreAPI.OverlayCanvas.Children.Add(_friendlyPanel);
            Canvas.SetTop(_friendlyPanel, Settings.Default.PlayerTop);
            Canvas.SetLeft(_friendlyPanel, Settings.Default.PlayerLeft);

            Input = new InputManager(_friendlyPanel);

            Settings.Default.PropertyChanged += SettingsChanged;
            SettingsChanged(null, null);

            // Connect events
            GameEvents.OnGameStart.Add(SetUpWinrates);
            GameEvents.OnPlayerDraw.Add(AddToMulligan);
            GameEvents.OnGameEnd.Add(Reset);
            GameEvents.OnPlayerMulligan.Add(AddToMulligan);
        }


        private void AddToMulligan(Card card)
        {
            Mulligan.HighlightCard(card);
            if (CoreAPI.Game.IsMulliganDone)
            {
                Reset();
            }
        }

        private void SettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            _friendlyPanel.RenderTransform = new ScaleTransform(Settings.Default.FriendlyScale / 100,
                Settings.Default.FriendlyScale / 100);
            _friendlyPanel.Opacity = Settings.Default.FriendlyOpacity / 100;
        }

        public void Dispose()
        {
            CoreAPI.OverlayCanvas.Children.Remove(_friendlyPanel);

            Input.Dispose();
        }

        /**
        * Clear then recreate all Views.
        */
        public void Reset()
        {
            _friendlyPanel.Children.Clear();
            Mulligan = new MulliganView {Label = {Visibility = Visibility.Hidden}};
            _friendlyPanel.Children.Add(Mulligan);
        }

        private void SetUpWinrates()
        {
            Reset();
            var shortId = ShortIdHelper.GetShortId(DeckList.Instance.ActiveDeck);
            //check to see if shortId is in the hsreplay_decks.cache if so go get data
            var pos = Array.IndexOf(HsReplayDataManager.Decks.AvailableDecks, shortId);
            var has = pos >= 0;
            if (has)
            {
                _winrates = CreateWinRatesDictionary(shortId);
                foreach (int key in _winrates.Keys)
                    Mulligan.Update(new Card(Cards.GetFromDbfId(key)), _winrates);

                Mulligan = new MulliganView { Label = { Visibility = Visibility.Hidden } };

                var label = new HearthstoneTextBlock
                {
                    FontSize = 16,
                    TextAlignment = TextAlignment.Center,
                    Text = "Deck Winrate: " + _deckWinrate
                };
                var margin = label.Margin;
                margin.Top = 20;
                label.Margin = margin;
                Mulligan.Children.Add(label);
                _friendlyPanel.Children.Add(Mulligan);
                Mulligan.Visibility = Visibility.Visible;
                Mulligan.MulliganWinratesCardList.Visibility = Visibility.Visible;
                Mulligan.Label.Visibility = Visibility.Visible;
            }
                
            

            //foreach (var winrate in _winrates.Keys)
            //{
            //    Mulligan.Update(new Card(HearthDb.Cards.GetFromDbfId(winrate)),_winrates );
            //}
            
            
        }

        private static Dictionary<int, double> CreateWinRatesDictionary(string shortid)
        {
                var  shortId = shortid;

                var url =
                "https://hsreplay.net/analytics/query/single_deck_mulligan_guide/?GameType=RANKED_STANDARD&RankRange=ALL&Region=ALL&deck_id=" +
                shortId;

                var uriDeck = new Uri(url);
                var mulliganrootObject = DownloadSerializedJsonData<RootObject>(uriDeck);
                var winrates = GetWinrates(mulliganrootObject);

                _deckWinrate = mulliganrootObject.series.metadata.base_winrate;

                return winrates;
            //TODO logic for opponent and rank here if premium

        }

        private static T DownloadSerializedJsonData<T>(Uri uri) where T : new()
        {
            using (var w = new WebClient())
            {
                var jsonData = string.Empty;
                // attempt to download JSON data as a string
                try
                {
                    jsonData = w.DownloadString(uri);
                }
                catch (Exception ex)
                {
                    Log.Error("Something went wrong with the download." +
                              ex.Message);
                }

                // if string with JSON data is not empty, deserialize it to class and return its instance 
                return !string.IsNullOrEmpty(jsonData) ? JsonConvert.DeserializeObject<T>(jsonData) : new T();
            }
        }

        private static Dictionary<int, double> GetWinrates(RootObject rootObject)
        {
            var results = rootObject.series.data.ALL.OrderByDescending(e => e.opening_hand_winrate)
                .Select(e => new {e.dbf_id, e.opening_hand_winrate});

            var dictionary = new Dictionary<int, double>();
            foreach (var result in results)
            {
                dictionary.Add(result.dbf_id, result.opening_hand_winrate);
            }
            return dictionary;
        }
    }


}