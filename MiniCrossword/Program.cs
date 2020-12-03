using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AlgorithmX;

namespace MiniCrossword
{
    internal class Program
    {
        private const string LETTERS = "abcdefghijklmnopqrstuvwxyz";

        public static void Main(string[] args)
        {
            var words = File.ReadAllLines("path/to/file/with/words.txt");
            int size = words[0].Length;
            int primaryColumns = 2 * size * size * LETTERS.Length;
            int totalColumns = primaryColumns + words.Length;
            
            var matrix = new List<List<int>>();

            for (int wordIndex = 0; wordIndex < words.Length; wordIndex++)
            {
                string word = words[wordIndex];
                
                if (word.Length != size)
                    throw new ArgumentException("Words must be same length!");

                // r[0, size] : → ↓
                for (int i = 0; i < size * 2; i++)
                {
                    matrix.Add(new List<int>());
                }

                for (int row = 0; row < size; row++)
                {
                    for (int col = 0; col < size; col++)
                    {
                        int cellAcross = row * size + col;
                        int cellShiftAcross = cellAcross * LETTERS.Length * 2;

                        int cellDown = col * size + row;
                        int cellShiftDown = cellDown * LETTERS.Length * 2;

                        for (int letterIndex = 0; letterIndex < LETTERS.Length; letterIndex++)
                        {
                            int letterAcrossShift = cellShiftAcross + letterIndex * 2;
                            int matrixAcrossRowIndex = matrix.Count - size * 2 + row * 2;
                            
                            // across
                            if (word[col] == LETTERS[letterIndex])
                            {
                                matrix[matrixAcrossRowIndex].Add(letterAcrossShift);
                            }
                            else
                            {
                                matrix[matrixAcrossRowIndex].Add(letterAcrossShift + 1);
                            }
                            
                            // down
                            int letterDownShift = cellShiftDown + letterIndex * 2;

                            if (word[col] == LETTERS[letterIndex])
                            {
                                matrix[matrixAcrossRowIndex + 1].Add(letterDownShift + 1);
                            }
                            else
                            {
                                matrix[matrixAcrossRowIndex + 1].Add(letterDownShift);
                            }
                        }
                    }
                }
                
                int wordColumn = primaryColumns + wordIndex;
                
                for (int i = matrix.Count - size * 2; i < matrix.Count; i++)
                {
                    matrix[i].Add(wordColumn);
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
                PrintSolution(solutions[0], size, words);
            }
        }

        private static void PrintSolution(int[] solution, int size, string[] words)
        {
            Console.WriteLine();

            // across - down
            string[] solutionWords = new string[solution.Length];
            int rowsByWord = size * 2;
            
            foreach (int matrixRowIndex in solution)
            {
                int wordIndex = matrixRowIndex / rowsByWord;
                int solutionWordIndex = matrixRowIndex - wordIndex * rowsByWord;
                
                solutionWords[solutionWordIndex] = words[wordIndex];
            }
            
            StringBuilder acrossWords = new StringBuilder("Across: ");
            StringBuilder downWords = new StringBuilder("Down: ");

            for (int i = 0; i < size; i++)
            {
                int rowIndex = i * 2;
                
                acrossWords.Append(solutionWords[rowIndex]);
                downWords.Append(solutionWords[rowIndex + 1]);

                if (i != size - 1)
                {
                    acrossWords.Append(", ");
                    downWords.Append(", ");
                }
            }
            
            Console.WriteLine(acrossWords);
            Console.WriteLine(downWords);

            for (int i = 0; i < size; i++)
            {
                Console.Write("|");
                
                foreach (char c in solutionWords[i * 2])
                {
                    Console.Write($"{c}|");
                }
                
                Console.WriteLine();
            }
        }
    }
}