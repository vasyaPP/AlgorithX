using System;
using System.Collections.Generic;
using AlgorithmX;

namespace Pentamino
{
    internal class Program
    {
        private const int FIGURES_AMOUNT = 12;
        private static readonly Figure[] Figures = new Figure[FIGURES_AMOUNT];
        private static readonly int[][] Fields =
        {
            // { rows, cols }
            new [] { 6, 10 },
            new [] { 5, 12 },
            new [] { 4, 15 },
            new [] { 3, 20 }
        };
        
        public static void Main(string[] args)
        {
            int fieldIndex = 3;
            
            int fieldRows = Fields[fieldIndex][0];
            int fieldCols = Fields[fieldIndex][1];

            InitFigures();
            
            var matrix = new List<List<int>>();

            for (int figureIndex = 0; figureIndex < Figures.Length; figureIndex++)
            {
                var figure = Figures[figureIndex];

                foreach (var variant in figure.Variants)
                {
                    for (int row = 0; row < fieldRows; row++)
                    {
                        for (int col = 0; col < fieldCols; col++)
                        {
                            // test that figure fit for cell
                            bool fit = true;
                            
                            foreach (var point in variant)
                            {
                                int x = col + point.X;

                                if (x < 0 || x >= fieldCols)
                                {
                                    fit = false;
                                    break;
                                }
                                
                                int y = row + point.Y;

                                if (y < 0 || y >= fieldRows)
                                {
                                    fit = false;
                                    break;
                                }
                            }
                            
                            if (!fit)
                                continue;
                            
                            var matrixRow = new List<int> { figureIndex };

                            foreach (var point in variant)
                            {
                                int r = row + point.Y;
                                int c = col + point.X;
                                
                                matrixRow.Add(FIGURES_AMOUNT + r * fieldCols + c);
                            }
                            
                            matrix.Add(matrixRow);
                        }
                    }
                }
            }

            int columns = FIGURES_AMOUNT + fieldRows * fieldCols;
            var dlx = new Dlx(columns);

            for (int i = 0; i < matrix.Count; i++)
            {
                dlx.AddRow(i, matrix[i]);
            }

            var solutions = dlx.Solve();
            
            Console.WriteLine("Solutions: " + solutions.Count);

            if (solutions.Count > 0)
            {
                PrintSolution(solutions[0], matrix, FIGURES_AMOUNT, fieldRows, fieldCols);
            }
        }

        private static void InitFigures()
        {
            // F (exclude duplicate solutions with onlyRotateOnce = true)
            Figures[0] = new Figure(new []
            {
                " ##",
                "## ",
                " # "
            }, 'F', ConsoleColor.Yellow, true);

            // I
            Figures[1] = new Figure(new []
            {
                "#",
                "#",
                "#",
                "#",
                "#"
            }, 'I', ConsoleColor.Magenta);

            // L
            Figures[2] = new Figure(new []
            {
                "# ",
                "# ",
                "# ",
                "##"
            }, 'L', ConsoleColor.Red);
            
            // N
            Figures[3] = new Figure(new []
            {
                " #",
                "##",
                "# ",
                "# "
            }, 'N', ConsoleColor.Cyan);
            
            // P
            Figures[4] = new Figure(new []
            {
                " #",
                "##",
                "##"
            }, 'P', ConsoleColor.Green);
            
            // T
            Figures[5] = new Figure(new []
            {
                "###",
                " # ",
                " # "
            }, 'T', ConsoleColor.Blue);
            
            // U
            Figures[6] = new Figure(new []
            {
                "# #",
                "###"
            }, 'U', ConsoleColor.DarkGray);
            
            // V
            Figures[7] = new Figure(new []
            {
                "#  ",
                "#  ",
                "###"
            }, 'V', ConsoleColor.Gray);
            
            // W
            Figures[8] = new Figure(new []
            {
                "#  ",
                "## ",
                " ##"
            }, 'W', ConsoleColor.DarkYellow);
            
            // X
            Figures[9] = new Figure(new []
            {
                " # ",
                "###",
                " # "
            }, 'X', ConsoleColor.DarkCyan);
            
            // Y
            Figures[10] = new Figure(new []
            {
                " #",
                "##",
                " #",
                " #"
            }, 'Y', ConsoleColor.DarkGreen);
            
            // Z
            Figures[11] = new Figure(new []
            {
                "## ",
                " # ",
                " ##"
            }, 'Z', ConsoleColor.DarkMagenta);
        }

        private static void PrintVariant(Point[] variant, int xShift)
        {
            Console.WriteLine();
            
            int shiftX = 5 + xShift;
            int shiftY = 3;
            
            Console.SetCursorPosition(xShift + 1, 2);

            for (int i = 0; i < 10; i++)
            {
                Console.Write(Math.Abs(-4 + i));
            }
            
            Console.WriteLine();

            foreach (var point in variant)
            {
                Console.SetCursorPosition(shiftX + point.X, shiftY + point.Y);
                Console.Write("#");
            }
            
            Console.WriteLine();
        }

        private static void PrintSolution(int[] solution, List<List<int>> matrix, int figuresAmount, int fieldRows, int fieldCols)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            
            for (int row = 0; row < fieldRows; row++)
            {
                for (int col = 0; col < fieldCols; col++)
                {
                    int cell = row * fieldCols + col;

                    foreach (int matrixRowIndex in solution)
                    {
                        var matrixRow = matrix[matrixRowIndex];
                        Figure figure = null;

                        for (int i = 1; i < matrixRow.Count; i++)
                        {
                            int matrixCell = matrixRow[i] - figuresAmount;

                            if (matrixCell == cell)
                            {
                                figure = Figures[matrixRow[0]];
                                break;
                            }
                            
                            if (matrixCell > cell)
                                break;
                        }
                        
                        if (figure == null)
                            continue;

                        Console.ForegroundColor = figure.Color;
                        Console.Write(figure.Character + " "); 
                        break;
                    }
                }
                
                Console.WriteLine();
            }
            
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}