using System;
using System.Collections.Generic;
using AlgorithmX;

namespace EightQueens
{
    internal class Program
    {
        private const int SIZE = 4;
        
        public static void Main(string[] args)
        {
            int primaryColumns = SIZE + SIZE;
            int diagonalConstraints = 2 * SIZE - 1 - 2;
            int secondaryColumns = diagonalConstraints * 2;

            var dlx = new Dlx(primaryColumns + secondaryColumns, primaryColumns);
            
            var matrix = new List<List<int>>();

            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    var r = new List<int> { row, col + SIZE };
                    matrix.Add(r);

                    int d = row + col;

                    if (d > 0 && d < 2 * (SIZE - 1))
                    {
                        r.Add(d - 1 + primaryColumns);
                    }

                    int rd = SIZE - 1 - row + col;

                    if (rd > 0 && rd < 2 * (SIZE - 1))
                    {
                        r.Add(rd - 1 + primaryColumns + diagonalConstraints);
                    }
                }
            }

            for (int i = 0; i < matrix.Count; i++)
            {
                dlx.AddRow(i, matrix[i]);
            }
            
            var solutions = dlx.Solve();
            
            Console.WriteLine("Solutions: " + solutions.Count);

            if (solutions.Count > 0)
                PrintSolution(solutions[0], matrix);
        }

        private static void PrintSolution(int[] solution, List<List<int>> matrix)
        {
            int[] resultCols = new int[SIZE];

            foreach (int matrixRowIndex in solution)
            {
                var matrixRow = matrix[matrixRowIndex];
                resultCols[matrixRow[0]] = matrixRow[1] - SIZE;
            }
            
            Console.WriteLine();

            foreach (int queenCol in resultCols)
            {
                Console.Write("|");
                
                for (int col = 0; col < SIZE; col++)
                {
                    if (col == queenCol)
                    {
                        Console.Write("x|");
                    }
                    else
                    {
                        Console.Write(" |");
                    }
                }
                
                Console.WriteLine();
            }
        }
    }
}