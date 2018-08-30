using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Controls;
using Hearthstone_Deck_Tracker.HsReplay.Utility;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;
using Deck = HearthMirror.Objects.Deck;

namespace Graveyard
{
    public class WinrateTracker
    {
        
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly int AlphabetLength = Alphabet.Length;

        private Dictionary<Card, HearthstoneTextBlock> _winrates = new Dictionary<Card, HearthstoneTextBlock>();

        public void Update(Card card, List<Card> cards, AnimatedCardList view)
        {
            
            var deckcode = GetShortId(CoreAPI.Game.CurrentSelectedDeck);

            Log.Error(deckcode);
            var count = (double) cards.Aggregate(0, (total, c) => total + c.Count);
            for (var i = 0; i < cards.Count; i++)
            {
                if (!_winrates.ContainsKey(cards[i]))
                {
                    var winrate = new HearthstoneTextBlock {FontSize = 18, TextAlignment = TextAlignment.Left};
                    if (view.Items.Count > i)
                    {
                        if ((view.Items.GetItemAt(i) as UserControl)?.Content is Grid grid)
                        {
                            grid.Width = 260;
                            ((Hearthstone_Deck_Tracker.Controls.Card) grid.Children[0]).HorizontalAlignment =
                                HorizontalAlignment.Right;
                            ((Rectangle) grid.Children[1]).Width = 260;
                            grid.Children.Add(winrate);
                        }
                    }

                    _winrates.Add(cards[i], winrate);
                }

                _winrates[cards[i]].Text = $"{Math.Round(cards[i].Count / count * 100)}%";
            }
            
            
            
            
            
        }
        
        public static string GetShortId(Deck deck)
        {
            if (deck == null || deck.Cards.Count == 0)
                return string.Empty;
            try
            {
                var ids = deck.Cards.SelectMany(c => Enumerable.Repeat(c.Id.ToString(), c.Count));
                var idString = string.Join(",", ids.OrderBy(x => x, new Utf8StringComperer()));
                var bytes = Encoding.UTF8.GetBytes(idString);
                var hash = MD5.Create().ComputeHash(bytes);
                var hex = BitConverter.ToString(hash).Replace("-", string.Empty);
                return IntToString(BigInteger.Parse("00" + hex, NumberStyles.HexNumber));
            }
            catch (Exception e)
            {
                Log.Error(e);
                return string.Empty;
            }
        }

        private static string IntToString(BigInteger number)
        {
            var sb = new StringBuilder();
            while (number > 0)
            {
                var mod = number % AlphabetLength;
                sb.Append(Alphabet[(int) mod]);
                number = number / AlphabetLength;
            }

            return sb.ToString();
        }
    }
}