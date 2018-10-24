using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Controls;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;

namespace MulliganWinrate
{
    public class MulliganView : StackPanel
    {
        public List<Card> Cards;
        public HearthstoneTextBlock Label;
        public AnimatedCardList View;
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
            View = new AnimatedCardList();
            Children.Add(View);
            Cards = new List<Card>();
        }

        public void Update(Card card, Dictionary<int, double> winrates)
        {
            View.Visibility = Visibility.Visible;
            Label.Visibility = Visibility.Visible;
            // Increment
            var match = Cards.FirstOrDefault(c => c.Name == card.Name);
            if (match == null)
            {
                Cards.Add(card);
                View.Update(Cards, false);
                _winrateTracker.Update(card, Cards, View, winrates);
            }

            // Update View
        }

        public void HighlightCard(Card card)
        {

            var cardPosition = _winrateTracker.GetPosition(card);
            if (cardPosition == -1)
            {
                return;
            }
            if ((View.Items.GetItemAt(cardPosition) as UserControl)?.Content is Grid grid)
            {
                var textblock = (HearthstoneTextBlock) grid.Children[2];
                textblock.Fill = new SolidColorBrush(Colors.Green);
            }

        }
    }
}