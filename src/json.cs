using System;
using System.Collections.Generic;

namespace MulliganWinrate
{
    public class Metadata
    {
        public string earliest_date { get; set; }
        public string latest_date { get; set; }
        public int total_games { get; set; }
        public double base_winrate { get; set; }
        public DRUID DRUID { get; set; }
        public HUNTER HUNTER { get; set; }
        public MAGE MAGE { get; set; }
        public PALADIN PALADIN { get; set; }
        public PRIEST PRIEST { get; set; }
        public ROGUE ROGUE { get; set; }
        public SHAMAN SHAMAN { get; set; }
        public WARLOCK WARLOCK { get; set; }
        public WARRIOR WARRIOR { get; set; }
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


    public class DRUID
    {
        public int total_games { get; set; }
        public double base_winrate { get; set; }
        public string earliest_date { get; set; }
        public string latest_date { get; set; }
    }

    public class HUNTER
    {
        public int total_games { get; set; }
        public double base_winrate { get; set; }
        public string earliest_date { get; set; }
        public string latest_date { get; set; }
    }

    public class MAGE
    {
        public int total_games { get; set; }
        public double base_winrate { get; set; }
        public string earliest_date { get; set; }
        public string latest_date { get; set; }
    }

    public class PALADIN
    {
        public int total_games { get; set; }
        public double base_winrate { get; set; }
        public string earliest_date { get; set; }
        public string latest_date { get; set; }
    }

    public class PRIEST
    {
        public int total_games { get; set; }
        public double base_winrate { get; set; }
        public string earliest_date { get; set; }
        public string latest_date { get; set; }
    }

    public class ROGUE
    {
        public int total_games { get; set; }
        public double base_winrate { get; set; }
        public string earliest_date { get; set; }
        public string latest_date { get; set; }
    }

    public class SHAMAN
    {
        public int total_games { get; set; }
        public double base_winrate { get; set; }
        public string earliest_date { get; set; }
        public string latest_date { get; set; }
    }

    public class WARLOCK
    {
        public int total_games { get; set; }
        public double base_winrate { get; set; }
        public string earliest_date { get; set; }
        public string latest_date { get; set; }
    }

    public class WARRIOR
    {
        public int total_games { get; set; }
        public double base_winrate { get; set; }
        public string earliest_date { get; set; }
        public string latest_date { get; set; }
    }

   

    public class DRUID2
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

    public class HUNTER2
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

    public class MAGE2
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

    public class PALADIN2
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

    public class PRIEST2
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

    public class ROGUE2
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

    public class SHAMAN2
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

    public class WARLOCK2
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

    public class WARRIOR2
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
        public List<DRUID2> DRUID { get; set; }
        public List<HUNTER2> HUNTER { get; set; }
        public List<MAGE2> MAGE { get; set; }
        public List<PALADIN2> PALADIN { get; set; }
        public List<PRIEST2> PRIEST { get; set; }
        public List<ROGUE2> ROGUE { get; set; }
        public List<SHAMAN2> SHAMAN { get; set; }
        public List<WARLOCK2> WARLOCK { get; set; }
        public List<WARRIOR2> WARRIOR { get; set; }
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
