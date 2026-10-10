namespace TDGachaGame
{
    public class Tile
    {
        public int Row
        {
            get;
            set;
        }
        public int Col
        {
            get;
            set;
        }
        public TileType Type
        {
            get;
            set;
        }

        public Tile(int row, int col, TileType type)
        {
            Row = row;
            Col= col;
            Type = type;
        }
    }
}