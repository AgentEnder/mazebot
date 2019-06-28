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
        const string BASE_URL = "https://api.noopschallenge.com";
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

        public struct solutionResponseJson
        {
            public string result;
            public string message;
            public int shortestSolutionLength;
            public int yourSolutionLength;
            public int elapsed;
            public string nextMaze;
        }

        struct jsonRace
        {
            public string message;
            public string nextMaze;
            public string certificate;
            public string mazePath;
        }
#pragma warning restore 0649

        private jsonResponse currentMaze;
        private int[,] currentMapData;
        public int[,] CurrentMapData { get => currentMapData; }

        public MazeBot() :this("/mazebot/random/"){}

        public MazeBot(string path)
        {
            currentMapData = getMaze(path);
        }

        public string getMapName()
        {
            return currentMaze.name;
        }

        public solutionResponseJson CheckSolution(List<coordinate> sln)
        {
            return JsonConvert.DeserializeObject<solutionResponseJson>(checkSolution(sln));
        }

        private string checkSolution(List<coordinate> sln)
        {
            string formatted = formatInstructions(sln);

            string url = BASE_URL + currentMaze.mazePath;
            solutionJson solutionObj = new solutionJson { directions = formatted };

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            string json = JsonConvert.SerializeObject(solutionObj);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
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
                return "";
            }
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

        private int[,] getMaze(string path)
        {
            WebRequest req = HttpWebRequest.Create(BASE_URL +path);

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

        public static void Race(IHeuristic heuristic)
        {
            List<List<coordinate>> all_paths = new List<List<coordinate>>(); //Paths taken during various mazes;
            List<int[,]> mazes = new List<int[,]>(); //Graphs in race

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(BASE_URL + "/mazebot/race/start");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            string json = "{\"login\":\"agentender\"}";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    jsonRace res = JsonConvert.DeserializeObject<jsonRace>(result);
                    while (res.nextMaze != null)
                    {
                        Console.WriteLine($"Starting Maze {mazes.Count}");
                        MazeBot m = new MazeBot(res.nextMaze);
                        Graph g = Graph.Graphify(m.CurrentMapData);
                        mazes.Add(m.CurrentMapData);
                        Solver s = new Solver(g, heuristic);
                        List<coordinate> path = s.getSteps();
                        all_paths.Add(path);
                        json = m.checkSolution(path);
                        res = JsonConvert.DeserializeObject<jsonRace>(json);
                    }
                    Console.WriteLine("\n" + res.message);
                    Console.WriteLine("\n" + "CERTIFICATE: " + BASE_URL + res.certificate);
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
                throw ex;
            }


            //Save all of the mazes
            string directory = $"race/{ DateTime.Now.ToFileTime()}/";
            Directory.CreateDirectory(directory);
            Console.WriteLine();
            for (int i = 0; i < mazes.Count; i++)
            {
                Console.WriteLine($"Saving Maze {i}");
                ImageSaver.SaveMazeImage(mazes[i], all_paths[i], 16, 8, $"{directory}/{i}");
            }
        }
    }
}
