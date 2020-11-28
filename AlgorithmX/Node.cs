namespace AlgorithmX
{
    public class Node
    {
        public Node Left;
        public Node Right;
        public Node Up;
        public Node Down;
        public Header Header;
        public readonly int RowIndex;

        public Node(Header header, int rowIndex)
        {
            Left = Right = Up = Down = this;
            Header = header;
            RowIndex = rowIndex;
        }

        public void InsertLeft(Node node)
        {
            node.Left = Left;
            node.Right = this;
            Left.Right = node;
            Left = node;
        }
        
        public void UnlinkLeftRight()
        {
            Left.Right = Right;
            Right.Left = Left;
        }

        public void ReLinkLeftRight()
        {
            Left.Right = this;
            Right.Left = this;
        }

        public void InsertUp(Node node)
        {
            node.Up = Up;
            node.Down = this;
            Up.Down = node;
            Up = node;
        }

        public void UnlinkUpDown()
        {
            Up.Down = Down;
            Down.Up = Up;
            Header.Size--;
        }

        public void RelinkUpDown()
        {
            Up.Down = this;
            Down.Up = this;
            Header.Size++;
        }
    }
}