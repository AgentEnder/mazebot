using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using External;

namespace MazeSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            //Run I races

            RunPQueueTests(iterations: 100);

            //Uncomment below to run random maze iterations

            //string dir = $"runs/{DateTime.Now.ToFileTime()}/";
            //Directory.CreateDirectory(dir);
            //for (int i = 0; i < 1; i++)
            //{
            //    MazeBot m = new MazeBot();
            //    Graph graph = Graph.Graphify(m.CurrentMapData);
            //    Solver s = new Solver(graph, new ManhattanDistance());
            //    List<Coordinate> solutionPath = s.GetSteps();
            //    ImageSaver.SaveMazeImage(m.CurrentMapData, s.GetSteps(), 32, 8, $"{dir}/{m.MapName}");
            //    Console.WriteLine(m.CheckSolution(solutionPath).message);
            //    Console.WriteLine(Utils.List2String<Coordinate>(s.GetSteps()));
            //}
            Console.ReadLine();
        }

        static void RunPQueueTests( int iterations = 10, bool draw = false)
        {
            for (int i = 0; i < iterations; i++)
            {
                DateTime start = DateTime.Now;
                Console.WriteLine($"Starting race {i} at {start} \n");
                MazeBot.Race(new ManhattanDistance(), new BinaryHeap<Solver.PriorityQueueItem>(), draw);
                DateTime f = DateTime.Now;
                Console.WriteLine($"Finished at {f}");
                double elapsed = ((f.ToFileTime() - start.ToFileTime()) / (double)10000000); //Filetime is in 100 nanosecond intervals
                if (draw)
                    Console.WriteLine($"\nSolving and Drawing took {elapsed} seconds");
                else
                    Console.WriteLine($"\nSolving took {elapsed} seconds");
                Console.WriteLine("===============================================\n");
            }
            for (int i = 0; i < iterations; i++)
            {
                DateTime start = DateTime.Now;
                Console.WriteLine($"Starting race {i} at {start} \n");
                MazeBot.Race(new ManhattanDistance(), new ListPriorityQueue<Solver.PriorityQueueItem>(), draw);
                DateTime f = DateTime.Now;
                Console.WriteLine($"Finished at {f}");
                double elapsed = ((f.ToFileTime() - start.ToFileTime())  / (double)10000000); //Filetime is in 100 nanosecond intervals
                if (draw)
                    Console.WriteLine($"\nSolving and Drawing took {elapsed} seconds");
                else
                    Console.WriteLine($"\nSolving took {elapsed} seconds");
                Console.WriteLine("===============================================\n");
            }
        }
    }
}
