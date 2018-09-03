using System;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Plugins;

namespace MulliganWinrate
{
    public class MulliganWinratePlugin : IPlugin
    {
        private MulliganWinrate _mulliganWinrateInstance;

        public string Author => "Devalish & AwesomAL";

        public string ButtonText => "Settings";

        public string Description => @"Displays mulligan winrates for the current selected deck based on HS replay stats";

        public MenuItem MenuItem => null;

        public string Name => "Mulligan Winrates";

        public void OnButtonPress()
        {
            SettingsView.Flyout.IsOpen = true;

        }

        public void OnLoad()
        {
            _mulliganWinrateInstance = new MulliganWinrate();
        }

        public void OnUnload()
        {
            _mulliganWinrateInstance.Dispose();
            _mulliganWinrateInstance = null;
        }

        public void OnUpdate()
        {
        }

        public Version Version => new Version(0, 0, 1);
    }
}