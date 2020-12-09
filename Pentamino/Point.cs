namespace Pentamino
{
    public struct Point
    {
        public readonly int X;
        public readonly int Y;
        
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"[x = {X} y = {Y}]";
        }
    }
}