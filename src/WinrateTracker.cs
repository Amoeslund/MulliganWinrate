using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Controls;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;

namespace MulliganWinrate
{
    public class WinrateTracker
    {
        private Dictionary<Card, HearthstoneTextBlock> _winratesToCardMapping = new Dictionary<Card, HearthstoneTextBlock>();

        public void Update(Card card, List<Card> cards, AnimatedCardList view, Dictionary<int, double> winrates)
        {
            for (var i = 0; i < cards.Count; i++)
            {
                if (!_winratesToCardMapping.ContainsKey(cards[i]))
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

                    _winratesToCardMapping.Add(cards[i], winrate);
                }

                winrates.TryGetValue(cards[i].DbfIf, out var winratePercentage);
                _winratesToCardMapping[cards[i]].Text = winratePercentage.ToString();
            }
        }

        public int GetPosition(Card card)
        {
            for (var i = 0; i < _winratesToCardMapping.Keys.Count; i++)
            {
                if (_winratesToCardMapping.Keys.ToList()[i].Id == card.Id)
                {
                    return i;
                }
            }

            return -1;
        }
        
    }
}