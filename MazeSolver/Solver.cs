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

            List<pqueueItem> spots = new List<pqueueItem>();
            List<coordinate> seen = new List<coordinate>();
            spots.Add(new pqueueItem
            {
                priority = 0,
                curr = graph.Start,
                path = new List<coordinate>() { graph.Start}
            });

            while (!spots[0].curr.Equals(graph.End))
            {
                List<coordinate> adjs = graph.GetAdjacencies(spots[0].curr);
                foreach (coordinate adj in adjs)
                {
                    if (seen.Contains(adj))
                    {
                        continue;
                    }
                    pqueueItem new_spot = new pqueueItem()
                    {
                        priority = spots[0].path.Count + _heuristic.Score(graph, adj),
                        curr = adj,
                        path = new List<coordinate>(spots[0].path)
                    };
                    new_spot.path.Add(adj);
                    spots.Add(new_spot);
                }
                seen.Add(spots[0].curr);
                spots.RemoveAt(0);
                spots.Sort((x, y) => (x.priority.CompareTo(y.priority)));

            }
            path = spots[0].path;

        }

        public List<coordinate> getSteps()
        {
            if (path == null)
            {
                Solve();
            }
            return path;
        }

        struct pqueueItem
        {
            public double priority;
            public coordinate curr;
            public List<coordinate> path;
        }
    }
}
