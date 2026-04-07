using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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
        private readonly MulliganHandOverlay _handOverlay = new MulliganHandOverlay();
        private readonly List<Card> _mulliganHand = new List<Card>();

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
            GameEvents.OnPlayerDraw.Add(OnCardDrawn);
            GameEvents.OnPlayerMulligan.Add(OnCardMulliganed);
            GameEvents.OnGameEnd.Add(Reset);
        }


        private void OnCardDrawn(Card card)
        {
            if (Mulligan == null) return;
            if (!CoreAPI.Game.IsMulliganDone && _winrates != null && _winrates.Count > 0)
            {
                _mulliganHand.Add(card);
                _handOverlay.Show(_mulliganHand, _winrates, _deckWinrate);
            }
            FinishMulliganEvent(card);
        }

        private void OnCardMulliganed(Card card)
        {
            if (Mulligan == null) return;
            var existing = _mulliganHand.Find(c => c.Id == card.Id);
            if (existing != null)
            {
                _mulliganHand.Remove(existing);
                _handOverlay.Show(_mulliganHand, _winrates, _deckWinrate);
            }
            FinishMulliganEvent(card);
        }

        private void FinishMulliganEvent(Card card)
        {
            if (Mulligan == null) return;
            Mulligan.HighlightCard(card);
            if (CoreAPI.Game.IsMulliganDone)
                Reset();
        }

        private void SettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            _friendlyPanel.RenderTransform = new ScaleTransform(Settings.Default.FriendlyScale / 100,
                Settings.Default.FriendlyScale / 100);
            _friendlyPanel.Opacity = Settings.Default.FriendlyOpacity / 100;
        }

        public void Dispose()
        {
            GameEvents.OnGameStart.Remove(SetUpWinrates);
            GameEvents.OnPlayerDraw.Remove(OnCardDrawn);
            GameEvents.OnPlayerMulligan.Remove(OnCardMulliganed);
            GameEvents.OnGameEnd.Remove(Reset);
            Settings.Default.PropertyChanged -= SettingsChanged;
            CoreAPI.OverlayCanvas.Children.Remove(_friendlyPanel);
            _handOverlay.Clear();
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
            _handOverlay.Clear();
            _mulliganHand.Clear();
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

                var label = new HearthstoneTextBlock
                {
                    FontSize = 16,
                    TextAlignment = TextAlignment.Center,
                    Text = "Deck Winrate: " + _deckWinrate.ToString("P1")
                };
                var margin = label.Margin;
                margin.Top = 20;
                label.Margin = margin;
                Mulligan.Children.Add(label);
                Mulligan.Visibility = Visibility.Visible;
                Mulligan.MulliganWinratesCardList.Visibility = Visibility.Visible;
                Mulligan.Label.Visibility = Visibility.Visible;
            }
            
            
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

        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };

        private static T DownloadSerializedJsonData<T>(Uri uri) where T : new()
        {
            var jsonData = string.Empty;
            try
            {
                var response = _httpClient.GetAsync(uri).GetAwaiter().GetResult();
                if (!response.IsSuccessStatusCode)
                {
                    Log.Error($"HSReplay returned {(int)response.StatusCode} {response.ReasonPhrase} for {uri}. " +
                              "The endpoint may require authentication or have changed.");
                    return new T();
                }
                jsonData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log.Error("HSReplay request failed: " + ex.Message);
            }

            return !string.IsNullOrEmpty(jsonData) ? JsonConvert.DeserializeObject<T>(jsonData) : new T();
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