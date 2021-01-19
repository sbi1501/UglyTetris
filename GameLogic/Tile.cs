namespace UglyTetris.GameLogic
{
    public class Tile
    {
        public Tile(string color="DimGray")
        {
            Color = color;
        }

        public string Color { get; private set; }
    }
}