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
            var isInTestMode = (args.Length > 0 && args[0].Trim().ToLower() == "test");
            var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{(isInTestMode ? "test" : "full")}.txt").ToList();

            NavigationInstructions = puzzleInputRaw[0];
            ParsePuzzleInput(puzzleInputRaw);

            Console.WriteLine($"* Network nodes: {NetworkNodes.Count:N0}; navigation instructions {NavigationInstructions.Length:N0}");

            // PartA();
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
            
            var startingNodes = NetworkNodes.Where(n => n.Value.EndsInA).Select(n => n.Value).ToList();

            Console.WriteLine($"** Nodes ending in A: {startingNodes.Count:N0}");

			// do a single pass for each to figure out how many steps it
			// takes to get to Z

			for (int n = 0; n < startingNodes.Count; n++)
			{
				while (!NetworkNodes[startingNodes[n].CurrentNode].EndsInZ)
				{
					foreach(var instruction in NavigationInstructions)
					{
						var node = NetworkNodes[startingNodes[n].CurrentNode];
						
						var nextNode = instruction == 'L'
							? node.NodeToLeft
							: node.NodeToRight;

						startingNodes[n].CurrentNode = nextNode;
						startingNodes[n].StepsToZ++;

						if (NetworkNodes[startingNodes[n].CurrentNode].EndsInZ)
							break;
					}
				}

				Console.WriteLine($"** Starting from node {startingNodes[n].NodeID} it took {startingNodes[n].StepsToZ:N0} steps to get to {startingNodes[n].CurrentNode}");
			}

            // find the lowest common multiple of all the values, using the highest value as the starting point
            ulong multiplier = 1;
            var maxSteps = startingNodes.Max(n => n.StepsToZ);
            var remainingSteps = startingNodes.Where(n => n.StepsToZ != maxSteps).Select(n => n.StepsToZ).ToList();
            bool areAllStepsMultiples = false;

            while (!areAllStepsMultiples)
            {
                areAllStepsMultiples = true;

                foreach (var steps in remainingSteps)
                {
                    areAllStepsMultiples = areAllStepsMultiples && (((maxSteps * multiplier) % steps) == 0);

                    if (!areAllStepsMultiples)
                    {
                        multiplier++;
                        break;
                    }
                }
            }

            Console.WriteLine($"** A multiplier of {multiplier:N0} against {maxSteps:N0} produced an LCM of {multiplier * maxSteps:N0}");
		}
	}

    internal class NetworkNode
    {
        public string NodeID { get; set; }
        public string NodeToLeft { get; set; }
        public string NodeToRight { get; set; }
        public string CurrentNode { get; set; }
        public bool EndsInA { get; set; }
        public bool EndsInZ { get; set; }
		public ulong StepsToZ { get; set; }

        public NetworkNode(string rawNodeDetails)
        {
            var details = rawNodeDetails.Split(" =(,)".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            NodeID = details[0];
            NodeToLeft = details[1];
            NodeToRight = details[2];
            EndsInA = NodeID[^1] == 'A';
            EndsInZ = NodeID[^1] == 'Z';
            CurrentNode = NodeID;
			StepsToZ = 0;
        }
    }
}
