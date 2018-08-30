using System.Windows;
using System.Windows.Controls;
using HDT.Plugins.Graveyard;
using Hearthstone_Deck_Tracker;
using MahApps.Metro.Controls;

namespace MulliganWinrate
{
	public partial class SettingsView : ScrollViewer
	{
		private static Flyout _flyout;

		public static Flyout Flyout
		{
			get
			{
				if (_flyout == null)
				{
					_flyout = CreateSettingsFlyout();
				}
				return _flyout;
			}
		}

		private static Flyout CreateSettingsFlyout()
		{
			var settings = new Flyout();
			settings.Position = Position.Left;
			Panel.SetZIndex(settings, 100);
			settings.Header = "Graveyard Settings";
			settings.Content = new SettingsView();
			Core.MainWindow.Flyouts.Items.Add(settings);
			return settings;
		}

		public SettingsView()
		{
			InitializeComponent();
			Settings.Default.PropertyChanged += (sender, e) => Settings.Default.Save();
		}

		private void BtnUnlock_Click (object sender, RoutedEventArgs e) {
			BtnUnlock.Content = global::MulliganWinrate.MulliganWinrate.Input.Toggle() ? "Lock Graveyards" : "Unlock Graveyards";
		}
	}
}
