using System;
using System.Collections.Generic;

namespace TDGachaGame
{
    public class PlayerState
    {
        public int Cash
        {
            get;
            private set;
        }

        public Dictionary<UnitId, int> OwnedShapeCounts
        {
            get;
        } = new Dictionary<UnitId, int>();

        public PlayerState(int startingCash = 50)
        {
            Cash = startingCash;
            foreach (ShapeColor color in Enum.GetValues(typeof(ShapeColor)))
            {
                OwnedShapeCounts[new UnitId(ShapeType.Line, color)] = 1;
                OwnedShapeCounts[new UnitId(ShapeType.Triangle, color)] = 1;
                OwnedShapeCounts[new UnitId(ShapeType.Square, color)] = 1;
                OwnedShapeCounts[new UnitId(ShapeType.Pentagon, color)] = 1;
                OwnedShapeCounts[new UnitId(ShapeType.Hexagon, color)] = 1;
            }
        }

        public void AddOwnedShape(UnitId unit, int count = 1)
        {
            if (count <= 0)
            {
                return;
            }

            OwnedShapeCounts[unit] = OwnedShapeCounts.TryGetValue(unit, out int existing)
                ? existing + count
                : count;
        }

        public bool TrySpend(int amount)
        {
            if (amount <= 0 || Cash < amount)
            {
                return false;
            }

            Cash -= amount;
            return true;
        }

        public void AddCash(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            Cash += amount;
        }
    }
}