using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace MazeSolver
{
    class MazeBot
    {
#pragma warning disable 0649
        struct jsonResponse
        {
            public string name;
            public string mazePath;
            public int[] startingPosition;
            public int[] endingPosition;
            public string message;
            public solutionJson exampleSolution;
            public string[,] map;
        }

        struct solutionJson
        {
            public string directions;
        }

        struct solutionResponseJson
        {
            public string result;
            public string message;
            public int shortestSolutionLength;
            public int yourSolutionLength;
            public int elapsed;
            public string nextMaze;
        }
#pragma warning restore 0649

        private jsonResponse currentMaze;
        private int[,] currentMapData;
        public int[,] CurrentMapData { get => currentMapData; }

        public MazeBot()
        {
            currentMapData = getMaze();
        }

        public string getMapName()
        {
            return currentMaze.name;
        }

        public bool checkSolution(List<coordinate> sln)
        {
            string formatted = formatInstructions(sln);

            string url = "https://api.noopschallenge.com" + currentMaze.mazePath;
            solutionJson solutionObj = new solutionJson { directions = formatted };

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            string json = JsonConvert.SerializeObject(solutionObj);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                Console.WriteLine(json);
                streamWriter.Write(json);
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                }
            }
            catch (System.Net.WebException ex)
            {
                var httpResponse = ex.Response;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                }
                return false;
            }
            
            return true;
        }

        private string formatInstructions(List<coordinate> moves)
        {
            string res = "";

            for (int i = 0; i < moves.Count-1; i++)
            {
                coordinate delta = new coordinate
                {
                    y = moves[i].x - moves[i + 1].x,
                    x = moves[i].y - moves[i + 1].y
                };

                if (delta.x < 0)
                {
                    res += "E";
                }
                else if (delta.x > 0)
                {
                    res += "W";
                }
                else if (delta.y < 0)
                {
                    res += "S";
                }
                else if (delta.y > 0)
                {
                    res += "N";
                }
            }

            return res;
        }

        private int[,] getMaze()
        {
            WebRequest req = HttpWebRequest.Create("https://api.noopschallenge.com/mazebot/random/");
            //WebRequest req = HttpWebRequest.Create("https://api.noopschallenge.com/mazebot/random/?minSize=20&maxSize=20");
            req.Method = "GET";
            WebResponse response = req.GetResponse();
            string jsonData = "";
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                jsonData = reader.ReadToEnd();
            }
            currentMaze = JsonConvert.DeserializeObject<jsonResponse>(jsonData);
            Console.WriteLine($"Maze Start: {currentMaze.startingPosition}");
            Console.WriteLine($"Maze End: {currentMaze.endingPosition}");
            return parseMazeToInts(currentMaze.map);
        }

        protected int[,] parseMazeToInts(string[,] original)
        {
            int w = original.GetLength(0);
            int h = original.GetLength(1);
            int[,] data = new int[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int value = 0;
                    switch (original[x, y])
                    {
                        case " ":
                            //Empty air, leave value of 0
                            break;
                        case "X":
                            //Impassable wall
                            value = 1;
                            break;
                        case "A":
                            //Starting Point
                            value = 2;
                            break;
                        case "B":
                            //Ending point
                            value = 3;
                            break;
                        default:
                            break;
                    }
                    data[x, y] = value;
                }
            }
            return data;
        }
    }
}
