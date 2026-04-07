using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;

namespace MulliganWinrate
{
    /// <summary>
    /// Displays colored mulligan winrate labels directly above each card in the
    /// player's opening hand on the HDT overlay canvas.
    /// </summary>
    public class MulliganHandOverlay
    {
        private readonly List<Border> _labels = new List<Border>();

        // Horizontal card-center positions as a fraction of canvas width.
        // These match Hearthstone's mulligan hand layout at standard 16:9.
        private static readonly double[] XCenters3 = { 0.335, 0.500, 0.665 };
        private static readonly double[] XCenters4 = { 0.270, 0.393, 0.607, 0.730 };

        // Vertical position of the label top (fraction of canvas height).
        // Placed just above the top edge of the mulligan cards (~50 % height).
        private const double YFraction = 0.455;

        // Total number of slots fixed at first Show() call; slots never change
        // during a mulligan even when cards are sent back.
        private int _totalSlots;

        /// <param name="hand">Cards still in hand (mulliganed cards removed).</param>
        /// <param name="totalSlots">Original hand size — fixes the x-center array for the session.</param>
        public void Show(IList<Card> hand, Dictionary<int, double> winrates, double deckWinrate, int totalSlots)
        {
            Clear();
            if (hand == null || winrates == null) return;

            _totalSlots = totalSlots > 0 ? totalSlots : hand.Count;

            var canvas = CoreAPI.OverlayCanvas;
            var canvasW = canvas.ActualWidth  > 0 ? canvas.ActualWidth  : 1920;
            var canvasH = canvas.ActualHeight > 0 ? canvas.ActualHeight : 1080;

            // Use the slot count from the original draw, not the current hand size,
            // so positions stay fixed while cards are being mulliganed away.
            var xCenters = _totalSlots <= 3 ? XCenters3 : XCenters4;

            for (var i = 0; i < hand.Count && i < xCenters.Length; i++)
            {
                if (!winrates.TryGetValue(hand[i].DbfId, out var wr)) continue;

                var label = CreateLabel(wr, deckWinrate);

                label.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                var labelW = label.DesiredSize.Width > 0 ? label.DesiredSize.Width : 90;

                Canvas.SetLeft(label, xCenters[i] * canvasW - labelW / 2);
                Canvas.SetTop(label, canvasH * YFraction);
                canvas.Children.Add(label);
                _labels.Add(label);
            }
        }

        public void Clear()
        {
            var canvas = CoreAPI.OverlayCanvas;
            foreach (var label in _labels)
                canvas.Children.Remove(label);
            _labels.Clear();
            _totalSlots = 0;
        }

        private static Border CreateLabel(double winrate, double deckWinrate)
        {
            var above = winrate >= deckWinrate;
            var color  = above
                ? Color.FromRgb(0x1F, 0x8A, 0x22)  // green
                : Color.FromRgb(0xBB, 0x22, 0x22);  // red

            var text = new TextBlock
            {
                Text           = $"{(above ? "▲" : "▼")} {winrate * 100:F1}%",
                Foreground     = Brushes.White,
                FontSize       = 14,
                FontWeight     = FontWeights.Bold,
                Padding        = new Thickness(6, 2, 6, 2),
                TextAlignment  = TextAlignment.Center
            };

            return new Border
            {
                Background      = new SolidColorBrush(Color.FromArgb(210, color.R, color.G, color.B)),
                BorderBrush     = new SolidColorBrush(Color.FromArgb(140, 255, 255, 255)),
                BorderThickness = new Thickness(1),
                CornerRadius    = new CornerRadius(3),
                Child           = text
            };
        }
    }
}
