using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Controls;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;

namespace MulliganWinrate
{
    public class WinrateTracker
    {
        private Dictionary<Card, HearthstoneTextBlock> _winrates = new Dictionary<Card, HearthstoneTextBlock>();

        public void Update(Card card, List<Card> cards, AnimatedCardList view, Dictionary<int, double> winrates)
        {
            for (var i = 0; i < cards.Count; i++)
            {
                if (!_winrates.ContainsKey(cards[i]))
                {
                    var winrate = new HearthstoneTextBlock {FontSize = 16, TextAlignment = TextAlignment.Left };
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

                winrates.TryGetValue(cards[i].DbfIf, out var winratePercentage);
                _winrates[cards[i]].Text = winratePercentage.ToString();
            }
        }
    }
}