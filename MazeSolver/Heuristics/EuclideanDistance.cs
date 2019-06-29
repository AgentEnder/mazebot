using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    class EuclideanDistance : IHeuristic
    {
        public double Score(Graph g, coordinate move)
        {
            coordinate delta = new coordinate
            {
                x = g.End.x - move.x,
                y = g.End.y - move.y
            };
            return Math.Sqrt(delta.x*delta.x+delta.y*delta.y);
        }
    }
}
