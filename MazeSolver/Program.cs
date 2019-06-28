using System;
using System.IO;
using System.Collections.Generic;
using System.Net;

namespace MazeSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            MazeBot.Race(new ManhattanDistance());
            //Directory.CreateDirectory("runs/");
            //for (int i = 0; i < 1000; i++)
            //{
            //    MazeBot m = new MazeBot();
            //    Graph graph = Graph.Graphify(m.CurrentMapData);
            //    Solver s = new Solver(graph, new ManhattanDistance());
            //    List<coordinate> solutionPath = s.getSteps();
            //    ImageSaver.SaveMazeImage(m.CurrentMapData, s.getSteps(), 32, 8, $"runs/{m.getMapName()}");
            //    Console.WriteLine(m.CheckSolution(solutionPath).message);
            //    //Console.WriteLine(List2String<coordinate>(s.getSteps()));
            //}
            Console.ReadLine();
        }

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

        public static string List2String<T>(List<T> list){
            string ret = "[";
            foreach (T item in list)
            {
                ret += $"{item},";
            }
            return $"{ret}]";
        }
    }
}
