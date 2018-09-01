using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HDT.Plugins.Graveyard;
using HearthDb.Deckstrings;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.HsReplay.Utility;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Newtonsoft.Json;
using Core = Hearthstone_Deck_Tracker.API.Core;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;
using Deck = HearthMirror.Objects.Deck;

namespace MulliganWinrate
{
    public class MulliganWinrate
    {
        // The views

        public MulliganView Mulligan;
        private StackPanel _friendlyPanel;
        

        public static InputManager Input;
        private Dictionary<int, double> _winrates;

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
            GameEvents.OnGameStart.Add(SetUpWinrates);
            GameEvents.OnPlayerDraw.Add(AddToMulligan);
            GameEvents.OnGameEnd.Add(Reset);
            DeckManagerEvents.OnDeckSelected.Add(d=> SetUpWinrates());

        }

        private void SetUpWinrates()
        {
            Mulligan = new MulliganView {Label = {Visibility = Visibility.Hidden}};
            _friendlyPanel.Children.Add(Mulligan);
            _winrates = CreateWinRatesDictionary();
        }

        private void AddToMulligan(Card card)
        {
            if (!Core.Game.IsMulliganDone)
            {
                Mulligan.Update(card, _winrates);
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


            Mulligan = new MulliganView {Label = {Visibility = Visibility.Hidden}};
            _friendlyPanel.Children.Add(Mulligan);
        }
        
        
        public static Dictionary<int, double> CreateWinRatesDictionary()
        {
            string shortId = ShortIdHelper.GetShortId(DeckList.Instance.ActiveDeck);

            Log.Error(shortId);

            var url =
                "https://hsreplay.net/analytics/query/single_deck_mulligan_guide/?GameType=RANKED_STANDARD&RankRange=ALL&Region=ALL&deck_id=" +
                shortId;

            Uri uriDeck = new Uri(url);
            var mulliganrootObject = _download_serialized_json_data<RootObject>(uriDeck);
            var winrates = GetWinrates(mulliganrootObject);

            Log.Error(winrates.Keys.Count.ToString());
            var deckWinrate = mulliganrootObject.series.metadata.base_winrate;
            return winrates;
        }

        private static T _download_serialized_json_data<T>(Uri uri, [Optional] string filename) where T : new()
        {
            using (var w = new WebClient())
            {
                var jsonData = string.Empty;
                // attempt to download JSON data as a string
                // also download to file 
                try
                {
                    //w.DownloadFileAsync(uri, filename);
                    //w.DownloadFileCompleted += (sender, e) => logger.Info(filename + " has completed downloading");

                    jsonData = w.DownloadString(uri);
                    w.DownloadStringCompleted += (sender, e) =>
                        Log.Info("string " + filename + " has completed downloading");
                }
                catch (Exception ex)
                {
                    Log.Error("Something went wrong with the download." +
                              ex.Message); // render the exception with ${exception}
                }

                // if string with JSON data is not empty, deserialize it to class and return its instance 
                return !string.IsNullOrEmpty(jsonData) ? JsonConvert.DeserializeObject<T>(jsonData) : new T();
            }
        }

        private static Dictionary<int, double> GetWinrates(RootObject rootObject)
        {
            var winrateDict = new Dictionary<int, double>();

            foreach (var all in rootObject.series.data.ALL)
            {
                if (!all.opening_hand_winrate.Equals(null))
                {
                    if (!winrateDict.TryGetValue(all.dbf_id, out var temp))
                    {
                        winrateDict.Add(all.dbf_id, all.opening_hand_winrate);
                    }
                }
            }

            return winrateDict;
        }


        
        
    }
    public class Metadata
    {
        public string earliest_date { get; set; }
        public string latest_date { get; set; }
        public int total_games { get; set; }
        public double base_winrate { get; set; }
    }

    public class ALL
    {
        public int dbf_id { get; set; }
        public int times_presented_in_initial_cards { get; set; }
        public int times_kept { get; set; }
        public double keep_percentage { get; set; }
        public int times_in_opening_hand { get; set; }
        public double opening_hand_winrate { get; set; }
        public int times_card_drawn { get; set; }
        public double winrate_when_drawn { get; set; }
        public int times_card_played { get; set; }
        public double avg_turn_played_on { get; set; }
        public double avg_turns_in_hand { get; set; }
        public double winrate_when_played { get; set; }
    }

    public class Data
    {
        public List<ALL> ALL { get; set; }
    }

    public class Series
    {
        public Metadata metadata { get; set; }
        public Data data { get; set; }
    }

    public class RootObject
    {
        public string render_as { get; set; }
        public Series series { get; set; }
        public DateTime as_of { get; set; }
    }
}