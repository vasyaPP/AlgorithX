using System;
using System.Collections.Generic;
using System.IO;
using AlgorithmX;

namespace Sudoku
{
    internal class Program
    {
        private const int SIZE = 9;
        
        public static void Main(string[] args)
        {
            var input = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample_map.txt"));
            
            if (input.Length != SIZE)
                throw new ArgumentException("Sudoku must has 9 rows!");
            
            int[] sourceMap = new int[SIZE * SIZE];
            int sourceSolutionRows = 0;

            for (int row = 0; row < input.Length; row++)
            {
                for (int col = 0; col < input[row].Length; col++)
                {
                    char c = input[row][col];

                    if (c != '#')
                    {
                        sourceMap[row * SIZE + col] = c - '0';
                        sourceSolutionRows++;
                    }
                }
            }
            
            var matrix = new List<List<int>>();
            int cells = SIZE * SIZE;
            int primaryColumns = cells + SIZE * SIZE + sourceSolutionRows;
            int totalColumns = primaryColumns +  SIZE * SIZE + SIZE * SIZE + 1;
            int solutionRowIndex = 0;

            for (int num = 0; num < SIZE; num++)
            {
                for (int row = 0; row < SIZE; row++)
                {
                    for (int col = 0; col < SIZE; col++)
                    {
                        // cell
                        int cell = row * SIZE + col;
                        var matrixRow = new List<int> { cell };
                        int shift = cells;
                
                        // square constraint
                        int squareRowIndex = row / 3;
                        int squareColIndex = col / 3;
                        int squareIndex = squareRowIndex * 3 + squareColIndex;
                        matrixRow.Add(shift + squareIndex * SIZE + num);
                        shift += SIZE * SIZE;
                        
                        // source solution
                        if (sourceMap[row * SIZE + col] == num + 1)
                        {
                            matrixRow.Add(shift + solutionRowIndex);
                            solutionRowIndex++;
                        }

                        shift += sourceSolutionRows;
                        
                        // row constraint
                        matrixRow.Add(shift + row * SIZE + num);
                        shift += SIZE * SIZE;
                        
                        // col constraint
                        matrixRow.Add(shift + col * SIZE + num);

                        matrix.Add(matrixRow);
                    }
                }
            }
            
            var dlx = new Dlx(totalColumns, primaryColumns);

            for (int i = 0; i < matrix.Count; i++)
            {
                dlx.AddRow(i, matrix[i]);
            }

            var solutions = dlx.Solve();
            
            Console.WriteLine("Solutions: " + solutions.Count);

            if (solutions.Count > 0)
            {
                PrintSolution(sourceMap, solutions[0], matrix);
            }
        }

        private static void PrintSolution(int[] sourceMap, int[] solution, List<List<int>> matrix)
        {
            Console.WriteLine();
            
            int cells = SIZE * SIZE;
            int[] solutionMap = new int[cells];

            foreach (int matrixRowIndex in solution)
            {
                int num = matrixRowIndex / cells + 1;
                int cellIndex = matrix[matrixRowIndex][0];
                solutionMap[cellIndex] = num;
            }
            
            for (int row = 0; row < SIZE; row++)
            {
                bool underline = (row + 1) % 3 == 0;
                
                if (underline)
                    Console.Write("\x1B[4m");

                for (int col = 0; col < SIZE; col++)
                {
                    int cellIndex = row * SIZE + col;
                    int solutionNum = solutionMap[cellIndex];
                    bool isSourceSolution = solutionNum == sourceMap[cellIndex];

                    Console.ForegroundColor = !isSourceSolution ? ConsoleColor.Green : ConsoleColor.Black;
                    Console.Write(solutionNum);
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write((col + 1) % 3 == 0 ? "|" : " ");
                }
                
                if (underline)
                    Console.Write("\x1B[0m");
                
                Console.WriteLine();
            }
        }
    }
}