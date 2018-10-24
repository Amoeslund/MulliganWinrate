using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Controls;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;

namespace MulliganWinrate
{
    public class MulliganView : StackPanel
    {
        public List<Card> Cards;
        public HearthstoneTextBlock Label;
        public AnimatedCardList MulliganWinratesCardList;
        private WinrateTracker _winrateTracker = new WinrateTracker();

        public MulliganView()
        {
            // Section Label

            Orientation = Orientation.Vertical;
            Label = new HearthstoneTextBlock
            {
                FontSize = 16, TextAlignment = TextAlignment.Center, Text = "Mulligan Winrates"
            };
            var margin = Label.Margin;
            margin.Top = 20;
            Label.Margin = margin;
            Children.Add(Label);

            // Card View
            MulliganWinratesCardList = new AnimatedCardList();
            Children.Add(MulliganWinratesCardList);
            Cards = new List<Card>();
        }

        public void Update(Card card, Dictionary<int, double> winrates)
        {
            MulliganWinratesCardList.Visibility = Visibility.Visible;
            Label.Visibility = Visibility.Visible;
            // Increment
            var match = Cards.FirstOrDefault(c => c.Name == card.Name);
            if (match == null)
            {
                Cards.Add(card);
                MulliganWinratesCardList.Update(Cards, false);
                _winrateTracker.Update(card, Cards, MulliganWinratesCardList, winrates);
            }

            // Update View
        }

        public void HighlightCard(Card card)
        {

            var cardPosition = _winrateTracker.GetPosition(card);
            MulliganWinratesCardList.Update(Cards, false);

            var match = Cards.FirstOrDefault(c => c.Name == card.Name);
            if (match != null)
            {
                Cards.Remove(card);
                card.HighlightInHand = true;
                MulliganWinratesCardList.Update(Cards, false);
            }
            
            for (var i = 0; i < ((AnimatedCardList) Children[1]).Items.Count; i++)
            {
                if ((((AnimatedCardList) Children[1]).Items.GetItemAt(i) as UserControl)?.Content is Grid grid2)
                {
                    ((HearthstoneTextBlock) grid2.Children[2]).Fill = new SolidColorBrush(Colors.Green);
                }
            }

            if ((MulliganWinratesCardList.Items.GetItemAt(cardPosition) as UserControl)?.Content is Grid grid)
            {
                var textblock = (HearthstoneTextBlock) grid.Children[2];
                textblock.Fill = new SolidColorBrush(Colors.Green);
                grid.Children[2] = textblock;
            }

            MulliganWinratesCardList.Update(Cards, false);

        }
    }
}