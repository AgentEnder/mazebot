using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    class Solver
    {
        private IHeuristic _heuristic;
        private Graph graph;
        private List<coordinate> path;
        public Solver(Graph g, IHeuristic heuristic)
        {
            _heuristic = heuristic;
            graph = g;
        }

        public void Solve()
        {

            //List<pqueueItem> spots = new List<pqueueItem>();
            BinaryHeap<pqueueItem> spots = new BinaryHeap<pqueueItem>();
            List<coordinate> seen = new List<coordinate>();
            spots.insert(new pqueueItem
            {
                priority = 0,
                curr = graph.Start,
                path = new List<coordinate>() { graph.Start}
            });

            pqueueItem current = spots.extractMin(); ;
            while (!current.curr.Equals(graph.End))
            {
                List<coordinate> adjs = graph.GetAdjacencies(current.curr);
                foreach (coordinate adj in adjs)
                {
                    if (seen.Contains(adj))
                    {
                        continue;
                    }
                    pqueueItem new_spot = new pqueueItem()
                    {
                        priority = current.path.Count + _heuristic.Score(graph, adj),
                        curr = adj,
                        path = new List<coordinate>(current.path)
                    };
                    new_spot.path.Add(adj);
                    spots.insert(new_spot);
                }
                seen.Add(current.curr);
                current = spots.extractMin();
            }
            path = current.path;

        }

        public List<coordinate> getSteps()
        {
            if (path == null)
            {
                Solve();
            }
            return path;
        }

        struct pqueueItem : IComparable<pqueueItem>
        {
            public double priority;
            public coordinate curr;
            public List<coordinate> path;

            public int CompareTo(pqueueItem other)
            {
                return priority.CompareTo(other.priority);
            }
        }
    }
}
