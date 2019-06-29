using System;
using System.Collections.Generic;
using System.Text;

namespace External
{
    static class Utils
    {
        public static void Print2DArray<T>(T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public static string List2String<T>(List<T> list)
        {
            string ret = "[";
            foreach (T item in list)
            {
                ret += $"{item},";
            }
            return $"{ret}]";
        }

        public static string Array2String<T>(T[] array)
        {
            string ret = "[";
            foreach (T item in array)
            {
                ret += item;
            }
            return $"{ret}]";
        }
    }
}
