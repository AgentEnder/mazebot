using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    class BreadthFirst : IHeuristic
    {
        public double Score(Graph g, Coordinate move)
        {
            //Solver considers depth of solution + heuristic score, 
            //when heuristic score is not considered the cost is increased 
            //by depth only leading to a breadth first approach.
            return 0; 
        }
    }
}
