using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    class ManhattanDistance : IHeuristic
    {
        public double Score(Graph g, Coordinate move)
        {
            return Math.Abs(g.End.x - move.x) + Math.Abs(g.End.y - move.y);
        }
    }
}
