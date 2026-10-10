using System.Collections.Generic;

namespace TDGachaGame
{
    public class SaveData
    {
        public int Version { get; set; } = 1;

        public int Coins { get; set; }

        public int Gems { get; set; }

        public int TowerFloor { get; set; } = 1;

        public int HighestFloorCleared { get; set; }

        public int MaxCollectionSlots { get; set; } = PlayerProfile.StartingCollectionSlots;
        public List<SavedUnit> Units { get; set; } = new List<SavedUnit>();

        public int[] Team { get; set; } = new int[] { -1, -1, -1, -1, -1, -1 };

        public List<string> RedeemedCodes { get; set; } = new List<string>();
    }
    public class SavedUnit
    {
        public string Shape { get; set; } = "";

        public string Color { get; set; } = "";

        public int Level { get; set; } = 1;

        public int Exp { get; set; }
    }
}