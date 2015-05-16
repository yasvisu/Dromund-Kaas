using System;
using System.Collections.Generic;

namespace DromundKaas
{
    class Utility
    {
        //Push list up and add newline at its bottom.
        public static void PushUp(List<char[]> data, char[] newline)
        {
            data.RemoveAt(0);
            data.Add(newline);
        }

        //Push list down and add newline at its top.
        public static void PushDown(List<char[]> data, char[] newline)
        {
            data.RemoveAt(data.Count - 1);
            data.Insert(0, newline);
        }

        public static string GenerateStarLine(Random gen)
        {
            char[] result = new char[GlobalVar.CONSOLE_WIDTH];
            for (int i = 0; i < result.Length; i++)
                result[i] = ' ';
            for (int i = 0; i < result.Length / 3; i++)
                result[gen.Next(0, result.Length)] = '.';
            return new string(result);
        }

    }
}