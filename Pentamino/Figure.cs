using System;
using System.Collections.Generic;

namespace Pentamino
{
    public class Figure
    {
        public readonly List<Point[]> Variants = new List<Point[]>();
        public readonly char Character;
        public readonly ConsoleColor Color;

        private const int POINTS_PER_FIGURE = 5;
        
        public Figure(string[] data, char character, ConsoleColor color, bool onlyRotateOnce = false)
        {
            Character = character;
            Color = color;
            
            int width = data[0].Length;
            int startCol = -1;
            Point[] variant = new Point[POINTS_PER_FIGURE];
            int pointIndex = 0;

            for (int row = 0; row < data.Length; row++)
            {
                if (data[row].Length != width)
                    throw new ArgumentException("Figure rows must be same length!");

                string dataRow = data[row];
                
                for (int col = 0; col < width; col++)
                {
                    if (dataRow[col] == ' ')
                        continue;

                    if (startCol == -1)
                        startCol = col;

                    int x = col - startCol;
                    
                    variant[pointIndex] = new Point(x, row);
                    pointIndex++;
                }
            }
            
            CreateVariants(variant, onlyRotateOnce);
        }

        private void CreateVariants(Point[] start, bool onlyRotateOnce)
        {
            TryPushVariant(start);

            for (int i = 0; i < 3; i++)
            {
                var rotated = RotateClockWise(Variants[i]);
                
                if (!TryPushVariant(rotated))
                    break;
                
                if (onlyRotateOnce)
                    return;
            }
            
            if (!TryPushVariant(FlipX(start)))
                return;

            int limit = Variants.Count + 2;
            
            for (int i = Variants.Count - 1; i < limit; i++)
            {
                var rotated = RotateClockWise(Variants[i]);
                
                if (!TryPushVariant(rotated))
                    return;
            }
        }

        private bool TryPushVariant(Point[] variant)
        {
            Array.Sort(variant, (a, b) =>
            {
                if (a.Y == b.Y)
                {
                    return a.X.CompareTo(b.X);
                }
            
                return a.Y.CompareTo(b.Y);
            });

            if (Variants.Count == 0)
            {
                Variants.Add(variant);
                return true;
            }
            
            foreach (var aVariant in Variants)
            {
                bool checkNext = false;
                
                for (var i = 0; i < aVariant.Length; i++)
                {
                    if (aVariant[i].X != variant[i].X || aVariant[i].Y != variant[i].Y)
                    {
                        checkNext = true;
                        break;
                    }
                }

                if (!checkNext)
                    return false;
            }

            Variants.Add(variant);
            return true;
        }

        private static Point[] FlipX(Point[] variant)
        {
            int topMaxX = 0;

            foreach (var point in variant)
            {
                if (point.Y != 0 || point.X <= topMaxX)
                    continue;

                topMaxX = point.X;
            }
            
            Point[] flippedVariant = new Point[variant.Length];

            for (var i = 0; i < variant.Length; i++)
            {
                var point = variant[i];
                flippedVariant[i] = new Point(point.X * -1 + topMaxX, point.Y);
            }

            return flippedVariant;
        }

        private static Point[] RotateClockWise(Point[] variant)
        {
            int minX = 0;
            int maxY = 0;
            Point leftPoint = new Point(0, 0);

            for (int i = 0; i < variant.Length; i++)
            {
                var p = variant[i];

                if (p.X < minX)
                    minX = p.X;

                if (p.Y > maxY)
                    maxY = p.Y;
                
                if (p.X > leftPoint.X)
                    continue;
                
                if (p.X == leftPoint.X && p.Y < leftPoint.Y)
                    continue;

                leftPoint = p;
            }

            Point[] rotatedVariant = new Point[variant.Length];

            for (int i = 0; i < variant.Length; i++)
            {
                var point = variant[i];

                int y = point.X - minX;
                int x = maxY - point.Y - (maxY - leftPoint.Y);
                
                rotatedVariant[i] = new Point(x, y);
            }

            return rotatedVariant;
        }
    }
}