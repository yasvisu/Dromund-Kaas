using System;

namespace DromundKaas
{
    /// <summary>
    /// Point structure to hold 2D coordinates.
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X;
        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y;

        /// <summary>
        /// Default Point constructor with two values.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Point(int x = 0, int y = 0)
        {
            this.X = x;
            this.Y = y;
        }
    }

    /// <summary>
    /// Utility function class.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Roll a given boolean matrix down.
        /// </summary>
        /// <param name="Target">The target matrix to be rolled.</param>
        public static void RollUp(bool[,] Target)
        {
            for (int j = 0; j < Target.GetLength(1); j++)
            {
                Target[0, j] = false;
            }
            for (int i = 1; i < Target.GetLength(0); i++)
            {
                for (int j = 0; j < Target.GetLength(1); j++)
                {
                    if (Target[i, j])
                    {
                        Target[i, j] = false;
                        Target[i - 1, j] = true;
                    }
                }
            }
        }

        /// <summary>
        /// Roll a given boolean matrix down.
        /// </summary>
        /// <param name="Target">The target matrix to be rolled.</param>
        public static void RollDown(bool[,] Target)
        {
            for (int j = 0; j < Target.GetLength(1); j++)
            {
                int temp = Target.GetLength(0) - 1;
                Target[temp, j] = false;
            }
            for (int i = 0; i < Target.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < Target.GetLength(1); j++)
                {
                    if (Target[i, j])
                    {
                        Target[i, j] = false;
                        Target[i + 1, j] = true;
                    }
                }
            }
        }

        /// <summary>
        /// Move given entity in given direction. Modifies the Entity Location parameter.
        /// </summary>
        /// <param name="ToMove"> Entity to move.</param>
        /// <param name="Direction">Direction in which to move the entity. Accepted directions: 'u' (up), 'd' (down), 'l' (left), 'r' (right).</param>
        public static void MoveEntity(Entity ToMove, char Direction)
        {
            int x = 0, y = 0;
            switch (Direction)
            {
                case 'u':
                    y = -1;
                    break;
                case 'd':
                    y = 1;
                    break;
                case 'l':
                    x = -1;
                    break;
                case 'r':
                    x = 1;
                    break;
                default:
                    break;
            }
            ToMove.Location.X += x;
            ToMove.Location.Y += y;
        }

        /// <summary>
        /// Draw a colored string of the specified color at the current cursor position.
        /// </summary>
        /// <param name="Text">The string to be drawn.</param>
        /// <param name="Foreground">The Foreground color to use.</param>
        /// <param name="Background">The Background color to use.</param>
        public static void DrawColoredString(string Text, ConsoleColor Foreground, ConsoleColor Background)
        {
            ConsoleColor OriginalFore = Console.ForegroundColor;
            ConsoleColor OriginalBack = Console.BackgroundColor;
            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;
            Console.Write(Text);
            Console.ForegroundColor = OriginalFore;
            Console.BackgroundColor = OriginalBack;
        }
    }
}