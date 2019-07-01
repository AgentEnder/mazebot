using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    class EuclideanDistance : IHeuristic
    {
        public double Score(Graph g, Coordinate move)
        {
            //Diff between coordinates
            Coordinate delta = move - g.End;
            return (delta.x*delta.x+delta.y*delta.y); // Pythagorean Distance
        }
    }
}
