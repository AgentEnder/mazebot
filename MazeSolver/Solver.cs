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
        public Solver(Graph g, IHeuristic heuristic)
        {
            _heuristic = heuristic;
            graph = g;
        }

        public void Solve()
        {

            BinaryHeap<pqueueItem> spots = new BinaryHeap<pqueueItem>(); //Spots remaining to check
            List<Coordinate> seen = new List<Coordinate>(); //Spots already seen. Dont add them again.
            //Add the starting point to the graph.
            spots.Insert(new pqueueItem
            {
                priority = 0,
                curr = graph.Start,
                path = new List<Coordinate>() { graph.Start}
            });

            pqueueItem current = spots.ExtractMin(); //Get the min value from pqueue.
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
                    pqueueItem new_spot = new pqueueItem()
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

        private struct pqueueItem : IComparable<pqueueItem>
        {
            public double priority;
            public Coordinate curr;
            public List<Coordinate> path;

            public int CompareTo(pqueueItem other)
            {
                return priority.CompareTo(other.priority);
            }
        }
    }
}
