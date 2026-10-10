namespace TDGachaGame
{
    public readonly record struct UnitId(ShapeType Type, ShapeColor Color)
    {
        public override string ToString() => $"{Color} {Type}";
    }
}