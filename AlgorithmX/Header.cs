namespace AlgorithmX
{
    public class Header : Node
    {
        public int Size;
        
        public Header() : base(null, -1)
        {
            Header = this;
        }
    }
}