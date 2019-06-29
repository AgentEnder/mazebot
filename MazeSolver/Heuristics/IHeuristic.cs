using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    interface IHeuristic
    {
        double Score(Graph g, Coordinate move);
    }
}
