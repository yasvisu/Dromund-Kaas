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


        public static bool IsValidPoint(Point P)
        {
            if (P.X >= 0 && P.X < GlobalVar.CONSOLE_WIDTH && P.Y >= 0 && P.Y < GlobalVar.CONSOLE_HEIGHT)
                return true;
            return false;
        }


        public static void PrintEntity(Entity e)
        {
            //Set text color.
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = e.Color;

            Point temp = e.Location;
            Console.SetCursorPosition(temp.X, temp.Y);
            for (int i = 0; i < e.Type.Sprite.GetLength(0); i++)
            {
                for (int j = 0; j < e.Type.Sprite.GetLength(1); j++)
                {
                    int AbsoluteX = temp.X + j,
                        AbsoluteY = temp.Y + i;
                    if (AbsoluteX >= 0 && AbsoluteX < GlobalVar.CONSOLE_WIDTH && AbsoluteY >= 0 && AbsoluteY < GlobalVar.CONSOLE_HEIGHT)
                    {
                        Console.Write(e.Type.Sprite[i, j]);
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