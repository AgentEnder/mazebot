using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    struct coordinate
    {
        public int x;
        public int y;

        public override string ToString()
        {
            return $"({x},{y})";
        }
    }

    class Graph
    {
        Dictionary<coordinate, List<coordinate>> raw_adjacencies;
        coordinate start;
        public coordinate Start { get => start; }
        coordinate end;
        public coordinate End { get => end; }
        public int Width { get => width; }
        public int Height { get => height;}

        private int width;
        private int height;

        static public Graph Graphify(int[,] mazeData)
        {
            Graph g = new Graph();
            g.raw_adjacencies = new Dictionary<coordinate, List<coordinate>>();
            int w = mazeData.GetLength(0);
            g.width = w;
            int h = mazeData.GetLength(1);
            g.height = h;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int point = mazeData[x, y];
                    coordinate coord = new coordinate { x = x, y = y };
                    if (!g.raw_adjacencies.ContainsKey(coord))
                        g.raw_adjacencies[coord] = new List<coordinate>();
                    if (point != 1)
                    {
                        if (point == 2)
                            g.start = coord;
                        if (point == 3)
                            g.end = coord;

                        if (x + 1 < w)
                        {
                            coordinate right = new coordinate { x = x + 1, y = y };
                            if (mazeData[x + 1, y] != 1) //Can move right
                            {
                                if (!g.raw_adjacencies.ContainsKey(right))
                                {
                                    g.raw_adjacencies[right] = new List<coordinate>();
                                }
                                g.raw_adjacencies[coord].Add(right);
                                g.raw_adjacencies[right].Add(coord);
                            }
                        }
                        if (y + 1 < h)
                        {
                            coordinate down = new coordinate { x = x, y = y + 1 };
                            if (mazeData[x, y + 1] != 1) //Can move down
                            {
                                if (!g.raw_adjacencies.ContainsKey(down))
                                {
                                    g.raw_adjacencies[down] = new List<coordinate>();
                                }
                                g.raw_adjacencies[coord].Add(down);
                                g.raw_adjacencies[down].Add(coord);
                            }
                        }

                    }
                }
            }

            return g;

        }

        public List<coordinate> GetAdjacencies(coordinate n)
        {
            return raw_adjacencies[n];
        }
    }
}
