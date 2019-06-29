using System.Collections.Generic;

namespace MazeSolver
{


    class Graph
    {
        Dictionary<Coordinate, List<Coordinate>> rawAdjacencies; //Adjacenies stored as node:connected format
        Coordinate start;
        public Coordinate Start { get => start; }
        Coordinate end;
        public Coordinate End { get => end; }
        public int Width { get => width; }
        public int Height { get => height; }

        private int width;
        private int height;

        /// <summary>
        /// Factory method to generate a graph from a 2D int[]
        /// </summary>
        /// <param name="mazeData">Maze Data in int[w,h] format</param>
        /// <returns></returns>
        static public Graph Graphify(int[,] mazeData) //Read int[,] to a Graph
        {
            Graph g = new Graph
            {
                rawAdjacencies = new Dictionary<Coordinate, List<Coordinate>>()
            };
            int w = mazeData.GetLength(0);
            g.width = w;
            int h = mazeData.GetLength(1);
            g.height = h;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int point = mazeData[x, y];
                    Coordinate coord = new Coordinate(x, y);
                    if (!g.rawAdjacencies.ContainsKey(coord))
                        g.rawAdjacencies[coord] = new List<Coordinate>();
                    if (point != 1)
                    {
                        if (point == 2)
                            g.start = coord;
                        if (point == 3)
                            g.end = coord;

                        if (x + 1 < w)
                        {
                            Coordinate right = new Coordinate(x + 1, y);
                            if (mazeData[x + 1, y] != 1) //Can move right
                            {
                                if (!g.rawAdjacencies.ContainsKey(right))
                                {
                                    g.rawAdjacencies[right] = new List<Coordinate>();
                                }
                                g.rawAdjacencies[coord].Add(right);
                                g.rawAdjacencies[right].Add(coord);
                            }
                        }
                        if (y + 1 < h)
                        {
                            Coordinate down = new Coordinate(x, y + 1);
                            if (mazeData[x, y + 1] != 1) //Can move down
                            {
                                if (!g.rawAdjacencies.ContainsKey(down))
                                {
                                    g.rawAdjacencies[down] = new List<Coordinate>();
                                }
                                g.rawAdjacencies[coord].Add(down);
                                g.rawAdjacencies[down].Add(coord);
                            }
                        }

                    }
                }
            }

            return g;

        }

        public List<Coordinate> GetAdjacencies(Coordinate n)
        {
            return rawAdjacencies[n];
        }
    }
}
