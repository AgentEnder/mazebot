using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    interface IHeuristic
    {
        int Score(Graph g, coordinate move);
    }
}
