using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    class Solver
    {
        private IHeuristic _heuristic;
        private Graph graph;
        private List<Coordinate> path;
        public static IPriorityQueue<PriorityQueueItem> priorityQueue;

        public Solver(Graph g) : this(g, new ManhattanDistance()) { }

        public Solver(Graph g, IHeuristic heuristic) : this(g, heuristic, new BinaryHeap<PriorityQueueItem>()) { }

        public Solver(Graph g, IHeuristic heuristic, IPriorityQueue<PriorityQueueItem> queue)
        {
            _heuristic = heuristic;
            graph = g;
            queue.Clear();
            priorityQueue = queue;
        }

        public void Solve()
        {

            IPriorityQueue<PriorityQueueItem> spots = priorityQueue;  //Spots remaining to check
            List<Coordinate> seen = new List<Coordinate>(); //Spots already seen. Dont add them again.
            //Add the starting point to the graph.
            spots.Insert(new PriorityQueueItem
            {
                priority = 0,
                curr = graph.Start,
                path = new List<Coordinate>() { graph.Start}
            });

            PriorityQueueItem current = spots.ExtractMin(); //Get the min value from pqueue.
            while (!current.curr.Equals(graph.End)) //Not to end yet.
            {
                List<Coordinate> adjs = graph.GetAdjacencies(current.curr); //Where all can I go?
                foreach (Coordinate adj in adjs)
                {
                    if (seen.Contains(adj)) //Already been here
                    {
                        continue;
                    }
                    //I could come here though
                    PriorityQueueItem new_spot = new PriorityQueueItem()
                    {
                        priority = current.path.Count + _heuristic.Score(graph, adj),
                        curr = adj,
                        path = new List<Coordinate>(current.path)
                    };
                    new_spot.path.Add(adj); //Path includes current node
                    spots.Insert(new_spot); //I'll check it 
                }
                seen.Add(current.curr); //Just checked you.
                current = spots.ExtractMin(); //Get next node.
            }
            path = current.path; //Found a path.

        }

        /// <summary>
        /// Get the solved path. If Solve has not been called, Solve and then return path.
        /// </summary>
        /// <returns>Path to end in Coordinate Slice Format</returns>
        public List<Coordinate> GetSteps()
        {
            if (path == null)
            {
                Solve();
            }
            return path;
        }

        public struct PriorityQueueItem : IComparable<PriorityQueueItem>
        {
            public double priority;
            public Coordinate curr;
            public List<Coordinate> path;

            public int CompareTo(PriorityQueueItem other)
            {
                return priority.CompareTo(other.priority);
            }
        }
    }
}
