using System;
using System.Collections.Generic;

namespace AlgorithmX
{
    public class Dlx
    {
        private readonly Header _root;
        private readonly Header[] _headers;
        private readonly Stack<int> _partialSolution;
        private readonly List<int[]> _solutions;

        public Dlx(int columns, int primaryColumns)
        {
            _root = new Header();
            _headers = new Header[columns];
            _partialSolution = new Stack<int>();
            _solutions = new List<int[]>();

            for (int i = 0; i < _headers.Length; i++)
            {
                var header = new Header();
                _headers[i] = header;
                
                if (i < primaryColumns)
                    _root.InsertLeft(header);
            }
        }

        public void AddRow(int rowIndex, IEnumerable<int> orderedColumns)
        {
            Node start = null;
            int lastHeaderIndex = -1;
            
            foreach (int headerIndex in orderedColumns)
            {
                if (headerIndex <= lastHeaderIndex)
                {
                    throw new ArgumentException("Column indices must be in ascending order.");
                }

                lastHeaderIndex = headerIndex;
                
                var header = _headers[headerIndex];
                var node = new Node(header, rowIndex);
                header.InsertUp(node);
                header.Size++;

                if (start == null)
                {
                    start = node;
                }
                else
                {
                    start.InsertLeft(node);
                }
            }
        }

        public List<int[]> Solve()
        {
            if (_root.Right == _root)
            {
                if (_partialSolution.Count > 0)
                    _solutions.Add(_partialSolution.ToArray());

                return _solutions;
            }

            Header head = (Header)_root.Right;

            int minSize = head.Size;

            for (Header jNode = (Header)head.Right; jNode != _root; jNode = (Header)jNode.Right)
            {
                if (jNode.Size < minSize)
                {
                    minSize = jNode.Size;
                    head = jNode;
                }
            }
            
            CoverColumn(head);

            for (Node rNode = head.Down; rNode != head; rNode = rNode.Down)
            {
                _partialSolution.Push(rNode.RowIndex);
                
                for (Node jNode = rNode.Right; jNode != rNode; jNode = jNode.Right)
                {
                    CoverColumn(jNode.Header);
                }
                
                Solve();
                _partialSolution.Pop();

                for (Node jNode = rNode.Left; jNode != rNode; jNode = jNode.Left)
                {
                    UncoverColumn(jNode.Header);
                }
            }
            
            UncoverColumn(head);
            return _solutions;
        }

        private void CoverColumn(Header header)
        {
            header.UnlinkLeftRight();

            for (Node iNode = header.Down; iNode != header; iNode = iNode.Down)
            {
                for (Node jNode = iNode.Right; jNode != iNode; jNode = jNode.Right)
                {
                    jNode.UnlinkUpDown();
                }
            }
        }
        
        private void UncoverColumn(Header header)
        {
            for (Node iNode = header.Up; iNode != header; iNode = iNode.Up)
            {
                for (Node jNode = iNode.Left; jNode != iNode; jNode = jNode.Left)
                {
                    jNode.RelinkUpDown();
                }
            }
            
            header.ReLinkLeftRight();
        }
    }
}