using System;
using System.IO;
using System.Linq;

namespace Day08
{
    internal class Program
    {
        private static Dictionary<string, NetworkNode> NetworkNodes = new Dictionary<string, NetworkNode>();
        private static string NavigationInstructions;

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023: Day 8");
            var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

            NavigationInstructions = puzzleInputRaw[0];
            ParsePuzzleInput(puzzleInputRaw);

            Console.WriteLine($"* Network nodes: {NetworkNodes.Count:N0}; navigation instructions {NavigationInstructions.Length:N0}");

            PartA();
            PartB();
        }

        private static void ParsePuzzleInput(List<string> puzzleInputRaw)
        {
            for (int i = 2; i < puzzleInputRaw.Count; i++)
            {
                var networkNode = new NetworkNode(puzzleInputRaw[i]);
                NetworkNodes.Add(networkNode.NodeID, networkNode);
            }
        }

        private static void PartA()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            var step = 0;
            var currentNode = "AAA";

            while (currentNode != "ZZZ")
            {
                foreach (var instruction in NavigationInstructions)
                {
                    var node = NetworkNodes[currentNode];
                    step++;

                    currentNode = instruction == 'L' 
                        ? NetworkNodes[currentNode].NodeToLeft
                        : NetworkNodes[currentNode].NodeToRight;

                    if (currentNode == "ZZZ")
                        break;
                }
            }

            Console.WriteLine($"** Took {step:N0} step(s) to get to ZZZ.");
        }

        private static void PartB()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");
        }
    }

    internal class NetworkNode
    {
        public string NodeID { get; set; }
        public string NodeToLeft { get; set; }
        public string NodeToRight { get; set; }

        public NetworkNode(string rawNodeDetails)
        {
            var details = rawNodeDetails.Split(" =(,)".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            NodeID = details[0];
            NodeToLeft = details[1];
            NodeToRight = details[2];
        }
    }
}
