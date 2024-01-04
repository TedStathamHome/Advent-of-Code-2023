using System;
using System.IO;
using System.Linq;

namespace Day10
{
    internal class Program
    {
		private static int mapSizeX;
		private static int mapSizeY;
		
		private static int startX;
		private static int startY;

		private static int stepsTaken;

		private const char startPos = 'S';

		private const char vertical = '│';
		private const char horizontal = '─';
		private const char bendNtoE = '└';
		private const char bendNtoW = '┘';
		private const char bendStoW = '┐';
		private const char bendStoE = '┌';
		private const char ground = '▓';

		private const char origVertical = '|';
		private const char origHorizontal = '-';
		private const char origBendNtoE = 'L';
		private const char origBendNtoW = 'J';
		private const char origBendStoW = '7';
		private const char origBendStoE = 'F';
		private const char origGround = '.';

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023: Day 10");
            var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

			mapSizeX = puzzleInputRaw[0].Length;
			mapSizeY = puzzleInputRaw.Count;

			Console.WriteLine($"Map is {mapSizeX:N0} x {mapSizeY:N0} (across/high)");

			var fullMap = puzzleInputRaw.Select(x => x
				.Replace(origVertical, vertical)
				.Replace(origHorizontal, horizontal)
				.Replace(origBendNtoE, bendNtoE)
				.Replace(origBendNtoW, bendNtoW)
				.Replace(origBendStoW, bendStoW)
				.Replace(origBendStoE, bendStoE)
				.Replace(origGround, ground)
				.ToCharArray()).ToList();
			
			for (int r = 0; r < fullMap.Count; r++)
			{
				Console.WriteLine($"{string.Join("", fullMap[r])}");
			}

			var simplifiedMap = SimplifyMap(puzzleInputRaw, fullMap);

			Console.WriteLine("\r\nSimplified map:");

			for (int r = 0; r < simplifiedMap.Count; r++)
			{
				Console.WriteLine($"{string.Join("", simplifiedMap[r])}");
			}

			Console.WriteLine($"\r\nTotal steps taken to traverse the pipe: {stepsTaken:N0}");

			PartA();
            PartB();
        }

		private static List<char[]> SimplifyMap(List<string> puzzleInputRaw, List<char[]> fullMap)
		{
			// map is in x/y coordinates, from the top left
			//   0 .. n
			// 0 +--------->  x axis (columns)
			// . |
			// . |
			// n |
			//   V  y axis (rows)
			for (int r = 0; r < puzzleInputRaw.Count; r++)
			{
				var positionOfS = puzzleInputRaw[r].IndexOf(startPos);

				if (positionOfS >= 0)
				{
					startX = positionOfS;
					startY = r;
					break;	// we've found the starting character, so exit early
				}
			}

			var map = new List<char[]>();

			for (int r = 0; r < fullMap.Count; r++ )
			{
				// initialize the rows to be ground
				map.Add((new string(ground, mapSizeX)).ToCharArray());
			}

			// fill in the starting position on the map
			map[startY][startX] = startPos;

			var pathX = startX;
			var pathY = startY;
			var pathEnteredFrom = '?';
			
			var possiblePipesToN = $"{vertical}{bendStoW}{bendStoE}";
			var possiblePipesToE = $"{horizontal}{bendNtoW}{bendStoW}";
			var possiblePipesToS = $"{vertical}{bendNtoE}{bendNtoW}";
			var possiblePipesToW = $"{horizontal}{bendNtoE}{bendStoE}";

			// determine the starting path and direction
			// allows for attempts to look outside of the array bounds
			var pipeToN = ((pathY - 1) < 0) ? '?' : fullMap[pathY - 1][pathX];
			var pipeToE = ((pathX + 1) >= mapSizeX) ? '?' : fullMap[pathY][pathX + 1];
			var pipeToS = ((pathY + 1) >= mapSizeY) ? '?' : fullMap[pathY + 1][pathX];
			var pipeToW = ((pathX - 1) < 0) ? '?' : fullMap[pathY][pathX - 1];

			Console.WriteLine($"Start is at: x:{startX:N0}, y:{startY:N0}");
			Console.WriteLine($"Pipes to: N->{pipeToN}, E->{pipeToE}, S->{pipeToS}, W->{pipeToW}");

			// work with the first direction we can move in from the starting position
			// the check is performed clockwise starting from the north side
			if (possiblePipesToN.Contains(pipeToN))
			{
				pathY--;
				pathEnteredFrom = 'S';
			}
			else if (possiblePipesToE.Contains(pipeToE))
			{
				pathX++;
				pathEnteredFrom = 'W';
			}
			else if (possiblePipesToS.Contains(pipeToS))
			{
				pathY++;
				pathEnteredFrom = 'N';
			}
			else if (possiblePipesToW.Contains(pipeToW))
			{
				pathX--;
				pathEnteredFrom = 'E';
			}

			// define what pipes we can travel to, from each cardinal direction,
			// and what direction we can continue in for each of those pipes
			var pipesWithSouthEntrance = $"{vertical}{bendStoW}{bendStoE}";
			var directionFromPipesWithSouthEntrance = "NWE";

			var pipesWithWestEntrance = $"{horizontal}{bendNtoW}{bendStoW}";
			var directionFromPipesWithWestEntrance = "ENS";

			var pipesWithNorthEntrance = $"{vertical}{bendNtoW}{bendNtoE}";
			var directionFromPipesWithNorthEntrance = "SWE";

			var pipesWithEastEntrance = $"{horizontal}{bendNtoE}{bendStoE}";
			var directionFromPipesWithEastEntrance = "WNS";

			var currentPipe = fullMap[pathY][pathX];
			int pipeIndex;
			char moveTo;

			// travel the pipe until we return to the starting point
			while (!(pathX == startX && pathY == startY))
			{
				stepsTaken++;

				// populate the simplified map
				map[pathY][pathX] = currentPipe;

				if (pathEnteredFrom == 'S')
				{
					pipeIndex = pipesWithSouthEntrance.IndexOf(currentPipe);
					moveTo = directionFromPipesWithSouthEntrance[pipeIndex];
				}
				else if (pathEnteredFrom == 'W')
				{
					pipeIndex = pipesWithWestEntrance.IndexOf(currentPipe);
					moveTo = directionFromPipesWithWestEntrance[pipeIndex];
				}
				else if (pathEnteredFrom == 'N')
				{
					pipeIndex = pipesWithNorthEntrance.IndexOf(currentPipe);
					moveTo = directionFromPipesWithNorthEntrance[pipeIndex];
				}
				else // pathEnteredFrom must be E
				{
					pipeIndex = pipesWithEastEntrance.IndexOf(currentPipe);
					moveTo = directionFromPipesWithEastEntrance[pipeIndex];
				}

				switch (moveTo)
				{
					case 'N':
						pathY--;
						pathEnteredFrom = 'S';
						break;

					case 'E':
						pathX++;
						pathEnteredFrom = 'W';
						break;

					case 'S':
						pathY++;
						pathEnteredFrom = 'N';
						break;

					default:	// moveTo must be W
						pathX--;
						pathEnteredFrom = 'E';
						break;
				}

				currentPipe = fullMap[pathY][pathX];
			}

			return map;
		}

        private static void PartA()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");
			var middleStep = stepsTaken / 2 + (stepsTaken % 2);
			Console.WriteLine($"** Middle step is: {middleStep:N0}");
        }

        private static void PartB()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");
        }
    }
}
