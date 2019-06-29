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
            //Run I races

            for (int i = 0; i < 10; i++)
            {
                DateTime start = DateTime.Now;
                Console.WriteLine($"Starting race {i} at {start} \n");
                MazeBot.Race(new ManhattanDistance());
                DateTime f = DateTime.Now;
                Console.WriteLine($"Finished at {f}");
                double elapsed = ((f.ToFileTime() - start.ToFileTime()) * 1 / 10000000); //Filetime is in 100 nanosecond intervals
                Console.WriteLine($"\nSolving and Drawing took {elapsed} seconds");
                Console.WriteLine("===============================================\n");
            }

            //Uncomment below to run random maze iterations

            //string dir = $"runs/{DateTime.Now.ToFileTime()}/";
            //Directory.CreateDirectory(dir);
            //for (int i = 0; i < 1000; i++)
            //{
            //    MazeBot m = new MazeBot();
            //    Graph graph = Graph.Graphify(m.CurrentMapData);
            //    Solver s = new Solver(graph, new ManhattanDistance());
            //    List<coordinate> solutionPath = s.getSteps();
            //    ImageSaver.SaveMazeImage(m.CurrentMapData, s.getSteps(), 32, 8, $"{dir}/{m.getMapName()}");
            //    Console.WriteLine(m.CheckSolution(solutionPath).message);
            //    //Console.WriteLine(List2String<coordinate>(s.getSteps()));
            //}
            Console.ReadLine();
        }
    }
}
