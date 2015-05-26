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
        /// Checks whether given Point is within the confines of the Console screen.
        /// </summary>
        /// <param name="Current">The point to be checked.</param>
        /// <returns></returns>
        public static bool IsValidPoint(Point Current)
        {
            if (Current.X >= 0 && Current.X < GlobalVar.CONSOLE_WIDTH && Current.Y >= 0 && Current.Y < GlobalVar.CONSOLE_HEIGHT)
                return true;
            return false;
        }

        /// <summary>
        /// Print given entity to the console.
        /// </summary>
        /// <param name="Current">The entity to be printed.</param>
        public static void PrintEntity(Entity Current)
        {
            //Set text color.
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = Current.Color;

            Point temp = Current.Location;
            Console.SetCursorPosition(temp.X, temp.Y);
            for (int i = 0; i < Current.Type.Sprite.GetLength(0); i++)
            {
                for (int j = 0; j < Current.Type.Sprite.GetLength(1); j++)
                {
                    int AbsoluteX = temp.X + j,
                        AbsoluteY = temp.Y + i;
                    if (AbsoluteX >= 0 && AbsoluteX < GlobalVar.CONSOLE_WIDTH && AbsoluteY >= 0 && AbsoluteY < GlobalVar.CONSOLE_HEIGHT)
                    {
                        Console.Write(Current.Type.Sprite[i, j]);
                    }
                }
                temp.Y++;
                if (temp.X >= 0 && temp.X < GlobalVar.CONSOLE_WIDTH && temp.Y >= 0 && temp.Y < GlobalVar.CONSOLE_HEIGHT)
                    Console.SetCursorPosition(temp.X, temp.Y);
            }
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = previous;
        }

    }
}