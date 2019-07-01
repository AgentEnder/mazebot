using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace MazeSolver
{
    /// <summary>
    /// This class is solely built for dealing with the noops challenge MazeBot API. 
    /// If you are working with other maze data, this is not the class for you.
    /// </summary>
    class MazeBot
    {
        const string BASE_URL = "https://api.noopschallenge.com"; //API Base URL
        #region jsonStructs
#pragma warning disable 0649 //Disable the waring that states that all of the json struct members are never set. They are set through JsonConvert.Deserialze
        struct jsonMapResponse
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

        public struct SolutionResponseJson
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
            public float elapsed;
            public string nextMaze;
            public string certificate;
            
        }
#pragma warning restore 0649
        #endregion

        private jsonMapResponse currentMaze; //Store full data for current maze.
        private readonly int[,] currentMapData;
        public string MapName { get => currentMaze.name; }
        public int[,] CurrentMapData { get => currentMapData; }

        public MazeBot() : this("/mazebot/random/") { } // get a random maze when not fed a path.

        public MazeBot(string path)
        {
            Console.WriteLine("Getting maze from: " + BASE_URL + path);
            currentMapData = getMaze(path);
        }

        /// <summary>
        /// Check the solution of the instance.
        /// </summary>
        /// <param name="sln">Coordinate Slice format</param>
        /// <returns>Deserialized object</returns>
        public SolutionResponseJson CheckSolution(List<Coordinate> sln)
        {
            return JsonConvert.DeserializeObject<SolutionResponseJson>(checkSolution(sln));
        }

        /// <summary>
        /// Check the solution for our maze
        /// </summary>
        /// <param name="sln">Coordinate slice format of sln</param>
        /// <returns>Json string data</returns>
        private string checkSolution(List<Coordinate> sln)
        {
            string formatted = formatInstructions(sln); //Get instructions in form of "NSEW"

            string url = BASE_URL + currentMaze.mazePath; //API endpoint
            solutionJson solutionObj = new solutionJson { directions = formatted }; //This makes Json serialization easy

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            string json = JsonConvert.SerializeObject(solutionObj);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) //Write the post data to the stream
            {
                streamWriter.Write(json);
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse(); //When the instructions are incorrect the server returns a 400 error
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }
            }
            catch (System.Net.WebException ex) //Catch 400 Error
            {
                var httpResponse = ex.Response;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd(); //Let it explain whats wrong
                    Console.WriteLine(formatted);
                    Console.WriteLine(result);
                    return result;
                }
            }
        }

        /// <summary>
        /// Format instructions for submission to API
        /// </summary>
        /// <param name="moves">Moves in the form of coordinates at time slice.</param>
        /// <returns></returns>
        private string formatInstructions(List<Coordinate> moves)
        {
            string res = "";

            for (int i = 0; i < moves.Count - 1; i++)
            {
                Coordinate delta = new Coordinate(
                    moves[i].x - moves[i + 1].x,
                    moves[i].y - moves[i + 1].y
                );

                //This looks weird... Our List<coordinates> is actually transposed from what the server expects
                if (delta.x < 0)
                {
                    res += "S";
                }
                else if (delta.x > 0)
                {
                    res += "N";
                }
                else if (delta.y < 0)
                {
                    res += "E";
                }
                else if (delta.y > 0)
                {
                    res += "W";
                }
            }

            return res;
        }

        private int[,] getMaze(string path)
        {
            WebRequest req = HttpWebRequest.Create(BASE_URL + path);

            req.Method = "GET";
            WebResponse response = req.GetResponse();
            string jsonData = "";
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                jsonData = reader.ReadToEnd(); //Get json as string from web server
            }
            currentMaze = JsonConvert.DeserializeObject<jsonMapResponse>(jsonData); //deserialize into an object
            return parseMazeToInts(currentMaze.map); //parse maze to allow for MazeSolver to work with it.
        }

        /// <summary>
        /// This function just substitutes int values for the char values the API returns
        /// </summary>
        /// <param name="original">2D string array returned when getting a new maze.</param>
        /// <returns></returns>
        private int[,] parseMazeToInts(string[,] original)
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

        /// <summary>
        /// Run the Race Routine from MazeBot API
        /// </summary>
        /// <param name="heuristic">What heuristic should be used</param>
        /// <param name="queue">What type of queue should be used for the race</param>
        public static void Race(IHeuristic heuristic, IPriorityQueue<Solver.PriorityQueueItem> queue, bool draw = false)
        {
            List<List<Coordinate>> all_paths = new List<List<Coordinate>>(); //Paths taken during various mazes;
            List<int[,]> mazes = new List<int[,]>(); //Graphs in race

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(BASE_URL + "/mazebot/race/start"); //API Endpoint to start the race
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            string json = "{\"login\":\"agentender\"}"; //Replace 'agentender' with your username if you want your cert to match.
            jsonRace res;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json); //Start race
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse(); //Get json string data for first maze
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    res = JsonConvert.DeserializeObject<jsonRace>(result);
                }
                while (res.nextMaze != null) //Theres more races!
                {
                    Console.WriteLine($"Starting Maze {mazes.Count}"); //Log the maze start, so the consoles not empty the whole time
                    MazeBot m = new MazeBot(res.nextMaze); //Get a maze
                    Graph g = Graph.Graphify(m.CurrentMapData); //Make a graph
                    mazes.Add(m.CurrentMapData); //Save current data for drawing later
                    Solver s = new Solver(g, heuristic); //Solve the maze
                    List<Coordinate> path = s.GetSteps();
                    all_paths.Add(path); //Save the solution for drawing
                    json = m.checkSolution(path); //Check your work
                    res = JsonConvert.DeserializeObject<jsonRace>(json); //Get next maze
                }
            }
            catch (System.Net.WebException ex) //Bad req some where
            {
                var httpResponse = ex.Response;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                }
                throw ex;
            }

            if (draw)
            {
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
            string filePath = $"race/{heuristic.GetType().Name + queue.GetType().Name}Certificates.csv";
            using (StreamWriter f = new StreamWriter(filePath, true))
            {
                string time = res.message;
                time = time.Substring(time.IndexOf("in ") + 3);
                time = time.Substring(0, time.IndexOf(" seconds"));
                f.WriteLine($"{time},{BASE_URL + res.certificate}");
            }

        }
    }
}