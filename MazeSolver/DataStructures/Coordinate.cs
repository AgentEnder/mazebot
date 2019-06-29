using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    class Coordinate : IEquatable<Coordinate>
    {
        public int x;
        public int y;

        public Coordinate(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public bool Equals(Coordinate other)
        {
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode(); //Get hash of the string representation.
        }

        public override string ToString()
        {
            return $"({x},{y})";
        }

        public static Coordinate operator-(Coordinate a, Coordinate b)
        {
            return new Coordinate(a.x - b.x, a.y - b.y);
        }

        
    }
}
