using System;
using System.Collections.Generic;

namespace TDGachaGame
{
    public class PlayerProfile
    {
        public const int TeamSize = 6;

        public event Action? Changed;

        private void RaiseChanged()
        {
            Changed?.Invoke();
        }
        public const int StartingCollectionSlots = 100;
        public const int AbsoluteMaxCollectionSlots = 500;
        public const int ExpansionStep = 10;

        public int MaxCollectionSlots
        {
            get;
            private set;
        } = StartingCollectionSlots;
        public int ExpansionsBought => (MaxCollectionSlots - StartingCollectionSlots) / ExpansionStep;

        public bool IsCollectionAtMaxSize => MaxCollectionSlots >= AbsoluteMaxCollectionSlots;
        public int NextExpansionCost => IsCollectionAtMaxSize ? 0 : 100 + 25 * ExpansionsBought;
        public bool TryExpandCollection(out string message)
        {
            if (IsCollectionAtMaxSize)
            {
                message = $"Your collection is already at the maximum of {AbsoluteMaxCollectionSlots} slots.";
                return false;
            }

            int cost = NextExpansionCost;

            if (!TrySpendGems(cost))
            {
                message = $"Not enough gems: the next expansion costs {cost:N0}, you have {Gems:N0}.";
                return false;
            }

            MaxCollectionSlots = Math.Min(AbsoluteMaxCollectionSlots, MaxCollectionSlots + ExpansionStep);
            message = $"Collection expanded to {MaxCollectionSlots} slots!  (-{cost:N0} gems)";
            RaiseChanged();
            return true;
        }
        public static readonly ShapeType[] PlayableShapes =
        {
            ShapeType.Line,
            ShapeType.Triangle,
            ShapeType.Square,
            ShapeType.Pentagon,
            ShapeType.Hexagon
        };

        public int Coins
        {
            get;
            private set;
        }
        public int Gems
        {
            get;
            private set;
        } = 500;

        public int TowerFloor
        {
            get;
            private set;
        } = DevConfig.StartingStage;

        public int HighestFloorCleared
        {
            get;
            private set;
        }

        private readonly List<UnitId> collection = new List<UnitId>();
        private readonly List<UnitProgress> progress = new List<UnitProgress>();
        private readonly int[] teamCopies = { -1, -1, -1, -1, -1, -1 };
        public IReadOnlyList<UnitId> Collection => collection;

        public bool IsCollectionFull => collection.Count >= MaxCollectionSlots;
        public int FreeCollectionSlots => MaxCollectionSlots - collection.Count;
        public bool TryAddUnit(UnitId unit)
        {
            if (IsCollectionFull)
            {
                return false;
            }

            collection.Add(unit);
            progress.Add(new UnitProgress());
            RaiseChanged();
            return true;
        }
        public UnitId?[] Team
        {
            get
            {
                var result = new UnitId?[TeamSize];

                for (int i = 0; i < TeamSize; i++)
                {
                    result[i] = teamCopies[i] >= 0 ? collection[teamCopies[i]] : (UnitId?)null;
                }

                return result;
            }
        }

        public PlayerProfile()
        {
            foreach (UnitId extra in DevConfig.ExtraStartingUnits)
            {
                TryAddUnit(extra);
            }
        }

        public bool IsTeamEmpty
        {
            get
            {
                foreach (int copy in teamCopies)
                {
                    if (copy >= 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        public int GetTeamCopy(int slot)
        {
            return slot >= 0 && slot < TeamSize ? teamCopies[slot] : -1;
        }
        public int FindInTeam(int copyIndex)
        {
            for (int i = 0; i < teamCopies.Length; i++)
            {
                if (teamCopies[i] == copyIndex)
                {
                    return i;
                }
            }

            return -1;
        }

        public int FirstEmptySlot()
        {
            for (int i = 0; i < teamCopies.Length; i++)
            {
                if (teamCopies[i] < 0)
                {
                    return i;
                }
            }

            return -1;
        }
        public void Equip(int copyIndex, int slot)
        {
            if (slot < 0 || slot >= TeamSize || copyIndex < 0 || copyIndex >= collection.Count)
            {
                return;
            }

            int from = FindInTeam(copyIndex);
            if (from == slot)
            {
                return;
            }

            int displaced = teamCopies[slot];
            teamCopies[slot] = copyIndex;

            if (from >= 0)
            {
                teamCopies[from] = displaced;
            }

            RaiseChanged();
        }

        public void Unequip(int slot)
        {
            if (slot >= 0 && slot < TeamSize)
            {
                teamCopies[slot] = -1;
                RaiseChanged();
            }
        }

        public void ClearTeam()
        {
            for (int i = 0; i < teamCopies.Length; i++)
            {
                teamCopies[i] = -1;
            }

            RaiseChanged();
        }
        public int GetLevel(int copyIndex)
        {
            return copyIndex >= 0 && copyIndex < progress.Count ? progress[copyIndex].Level : 0;
        }
        public int GetExp(int copyIndex)
        {
            return copyIndex >= 0 && copyIndex < progress.Count ? progress[copyIndex].Exp : 0;
        }
        public int GetTeamLevel(int slot)
        {
            return GetLevel(GetTeamCopy(slot));
        }
        public int GetBestLevel(UnitId unit)
        {
            int best = 1;

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i] == unit)
                {
                    best = Math.Max(best, progress[i].Level);
                }
            }

            return best;
        }

        public bool TrySpendCoins(int amount)
        {
            if (amount <= 0 || Coins < amount)
            {
                return false;
            }

            Coins -= amount;
            RaiseChanged();
            return true;
        }
        private void AddExp(int copyIndex, int amount)
        {
            UnitProgress unit = progress[copyIndex];
            unit.Exp += amount;

            while (unit.Level < CollectionUpgradeConfig.MaxLevel && unit.Exp >= CollectionUpgradeConfig.GetExpToNext(unit.Level))
            {
                unit.Exp -= CollectionUpgradeConfig.GetExpToNext(unit.Level);
                unit.Level++;
            }

            if (unit.Level >= CollectionUpgradeConfig.MaxLevel)
            {
                unit.Level = CollectionUpgradeConfig.MaxLevel;
                unit.Exp = 0;
            }
        }
        public bool TryLevelUpWithCoins(int copyIndex, out string message)
        {
            if (copyIndex < 0 || copyIndex >= collection.Count)
            {
                message = "Pick a unit first.";
                return false;
            }

            UnitId unit = collection[copyIndex];
            UnitProgress state = progress[copyIndex];

            if (state.Level >= CollectionUpgradeConfig.MaxLevel)
            {
                message = $"{unit} is already at the max level.";
                return false;
            }

            int cost = CollectionUpgradeConfig.GetLevelUpCost(unit, state.Level);

            if (!TrySpendCoins(cost))
            {
                message = $"Not enough coins: leveling up costs {cost:N0}, you have {Coins:N0}.";
                return false;
            }
            state.Level++;
            state.Exp = 0;

            if (state.Level >= CollectionUpgradeConfig.MaxLevel)
            {
                state.Level = CollectionUpgradeConfig.MaxLevel;
            }

            message = $"{unit} is now level {state.Level}!  (-{cost:N0} coins)";
            RaiseChanged();
            return true;
        }
        public int GetMergeExp(IEnumerable<int> copies)
        {
            int total = 0;

            foreach (int copy in copies)
            {
                if (copy >= 0 && copy < collection.Count)
                {
                    total += CollectionUpgradeConfig.GetMergeExp(collection[copy]);
                }
            }

            return total;
        }
        public bool TryMerge(int targetIndex, IEnumerable<int> fodderCopies, out int newTargetIndex, out string message)
        {
            newTargetIndex = targetIndex;

            var fodder = new List<int>();
            foreach (int copy in fodderCopies)
            {
                if (copy != targetIndex && copy >= 0 && copy < collection.Count && !fodder.Contains(copy))
                {
                    fodder.Add(copy);
                }
            }

            if (targetIndex < 0 || targetIndex >= collection.Count)
            {
                message = "Pick the unit you want to upgrade first.";
                return false;
            }

            if (fodder.Count == 0)
            {
                message = "Pick at least one unit to merge into it.";
                return false;
            }

            if (progress[targetIndex].Level >= CollectionUpgradeConfig.MaxLevel)
            {
                message = $"{collection[targetIndex]} is already at the max level.";
                return false;
            }

            int exp = GetMergeExp(fodder);
            UnitId targetUnit = collection[targetIndex];

            int before = progress[targetIndex].Level;
            AddExp(targetIndex, exp);
            int after = progress[targetIndex].Level;

            int shift = 0;
            foreach (int copy in fodder)
            {
                if (copy < targetIndex)
                {
                    shift++;
                }
            }

            RemoveCopies(fodder);
            newTargetIndex = targetIndex - shift;

            message = after > before
                ? $"{targetUnit} grew from level {before} to level {after}!  (+{exp:N0} exp, {fodder.Count} merged)"
                : $"{targetUnit} gained {exp:N0} exp.  ({fodder.Count} merged)";
            RaiseChanged();
            return true;
        }
        public int SellCopies(IEnumerable<int> copies)
        {
            var toSell = new List<int>();
            int coins = 0;

            foreach (int copy in copies)
            {
                if (copy >= 0 && copy < collection.Count && !toSell.Contains(copy))
                {
                    toSell.Add(copy);
                    coins += CollectionUpgradeConfig.GetSellValue(collection[copy], progress[copy].Level);
                }
            }

            RemoveCopies(toSell);
            AddCoins(coins);
            RaiseChanged();
            return coins;
        }

        public int GetSellValue(int copyIndex)
        {
            if (copyIndex < 0 || copyIndex >= collection.Count)
            {
                return 0;
            }

            return CollectionUpgradeConfig.GetSellValue(collection[copyIndex], progress[copyIndex].Level);
        }
        private void RemoveCopies(IEnumerable<int> indices)
        {
            var sorted = new List<int>();
            foreach (int index in indices)
            {
                if (index >= 0 && index < collection.Count && !sorted.Contains(index))
                {
                    sorted.Add(index);
                }
            }
            sorted.Sort();
            sorted.Reverse();

            foreach (int index in sorted)
            {
                for (int t = 0; t < teamCopies.Length; t++)
                {
                    if (teamCopies[t] == index)
                    {
                        teamCopies[t] = -1;
                    }
                    else if (teamCopies[t] > index)
                    {
                        teamCopies[t]--;
                    }
                }

                collection.RemoveAt(index);
                progress.RemoveAt(index);
            }
        }
        public SaveData ToSaveData()
        {
            var data = new SaveData
            {
                Coins = Coins,
                Gems = Gems,
                TowerFloor = TowerFloor,
                HighestFloorCleared = HighestFloorCleared,
                MaxCollectionSlots = MaxCollectionSlots,
                Team = (int[])teamCopies.Clone()
            };

            for (int i = 0; i < collection.Count; i++)
            {
                data.Units.Add(new SavedUnit
                {
                    Shape = collection[i].Type.ToString(),
                    Color = collection[i].Color.ToString(),
                    Level = progress[i].Level,
                    Exp = progress[i].Exp
                });
            }

            data.RedeemedCodes.AddRange(RedeemedCodes);
            return data;
        }
        public bool LoadFromSaveData(SaveData data)
        {
            if (data == null || data.Units == null)
            {
                return false;
            }

            var units = new List<UnitId>();
            var levels = new List<UnitProgress>();
            var newIndex = new Dictionary<int, int>();

            for (int i = 0; i < data.Units.Count && units.Count < AbsoluteMaxCollectionSlots; i++)
            {
                SavedUnit saved = data.Units[i];

                if (saved == null
                    || !Enum.TryParse(saved.Shape, out ShapeType type)
                    || !Enum.TryParse(saved.Color, out ShapeColor color)
                    || Array.IndexOf(PlayableShapes, type) < 0
                    || !Enum.IsDefined(typeof(ShapeColor), color))
                {
                    continue;
                }

                int level = Math.Max(1, Math.Min(saved.Level, CollectionUpgradeConfig.MaxLevel));
                int exp = 0;

                if (level < CollectionUpgradeConfig.MaxLevel)
                {
                    exp = Math.Max(0, Math.Min(saved.Exp, CollectionUpgradeConfig.GetExpToNext(level) - 1));
                }

                newIndex[i] = units.Count;
                units.Add(new UnitId(type, color));
                levels.Add(new UnitProgress { Level = level, Exp = exp });
            }
            int slots = Math.Max(StartingCollectionSlots, Math.Min(data.MaxCollectionSlots, AbsoluteMaxCollectionSlots));
            slots = StartingCollectionSlots + (slots - StartingCollectionSlots) / ExpansionStep * ExpansionStep;

            while (slots < units.Count && slots < AbsoluteMaxCollectionSlots)
            {
                slots += ExpansionStep;
            }
            var team = new int[TeamSize];
            Array.Fill(team, -1);

            for (int t = 0; t < TeamSize; t++)
            {
                if (data.Team != null && t < data.Team.Length && newIndex.TryGetValue(data.Team[t], out int copy)
                    && Array.IndexOf(team, copy) < 0)
                {
                    team[t] = copy;
                }
            }

            collection.Clear();
            collection.AddRange(units);
            progress.Clear();
            progress.AddRange(levels);
            Array.Copy(team, teamCopies, TeamSize);

            MaxCollectionSlots = slots;
            Coins = Math.Max(0, data.Coins);
            Gems = Math.Max(0, data.Gems);
            TowerFloor = Math.Max(1, data.TowerFloor);
            HighestFloorCleared = Math.Max(0, data.HighestFloorCleared);

            RedeemedCodes.Clear();
            if (data.RedeemedCodes != null)
            {
                foreach (string code in data.RedeemedCodes)
                {
                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        RedeemedCodes.Add(code);
                    }
                }
            }

            return true;
        }
        public static int GetFloorReward(int floor)
        {
            return 500 + (Math.Max(0, floor) / 5) * 50;
        }
        public static int GetFloorGemReward(int floor)
        {
            return 100 + (Math.Max(0, floor) / 5) * 10;
        }

        public void AddCoins(int amount)
        {
            if (amount > 0)
            {
                Coins += amount;
                RaiseChanged();
            }
        }

        public void AddGems(int amount)
        {
            if (amount > 0)
            {
                Gems += amount;
                RaiseChanged();
            }
        }

        public bool TrySpendGems(int amount)
        {
            if (amount <= 0 || Gems < amount)
            {
                return false;
            }

            Gems -= amount;
            RaiseChanged();
            return true;
        }
        public HashSet<string> RedeemedCodes
        {
            get;
        } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public int CompleteTowerFloor(int floor, out int gemReward)
        {
            int reward = GetFloorReward(floor);
            gemReward = GetFloorGemReward(floor);
            Coins += reward;
            Gems += gemReward;
            HighestFloorCleared = Math.Max(HighestFloorCleared, floor);
            TowerFloor = floor + 1;
            RaiseChanged();
            return reward;
        }
    }
    public class UnitProgress
    {
        public int Level = 1;
        public int Exp;
    }
}