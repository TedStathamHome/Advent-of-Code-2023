using System;
using System.IO;
using System.Linq;

namespace Day10
{
    internal class Program
    {
		private static int mapSizeX;
		private static int mapSizeY;
		
		private static int pathAStartX;
		private static int pathAStartY;
		private static char pathAStartEnteredFrom;
		
		private static int pathBStartX;
		private static int pathBStartY;
		private static char pathBStartEnteredFrom;

		private static int startX;
		private static int startY;

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

			var pathAx = startX;
			var pathAy = startY;
			var pathAEnteredFrom = '?';
			
			var pathBx = startX;
			var pathBy = startY;
			var pathBEnteredFrom = '?';

			var currentPath = 'A';
			var possiblePipesToN = $"{vertical}{bendStoW}{bendStoE}";
			var possiblePipesToE = $"{horizontal}{bendNtoW}{bendStoW}";
			var possiblePipesToS = $"{vertical}{bendNtoE}{bendNtoW}";
			var possiblePipesToW = $"{horizontal}{bendNtoE}{bendStoE}";

			// initialize the A and B paths

			// assume the starting point isn't at 0,0, meaning
			// there should be map points to the N, E, S, and W
			var pipeToN = fullMap[startY - 1][startX];
			var pipeToE = fullMap[startY][startX + 1];
			var pipeToS = fullMap[startY + 1][startX];
			var pipeToW = fullMap[startY][startX - 1];

			if (possiblePipesToN.Contains(pipeToN))
			{
				pathAx = startX;
				pathAy = startY - 1;
				pathAEnteredFrom = 'S';
				currentPath = 'B';
			}

			if (possiblePipesToE.Contains(pipeToE))
			{
				if (currentPath == 'A')
				{
					pathAx = startX + 1;
					pathAy = startY;
					pathAEnteredFrom = 'W';
					currentPath = 'B';
				}
				else if (currentPath == 'B')
				{
					pathBx = startX + 1;
					pathBy = startY;
					pathBEnteredFrom = 'W';
					currentPath = '?';
				}
			}

			if (possiblePipesToS.Contains(pipeToS))
			{
				if (currentPath == 'A')
				{
					pathAx = startX;
					pathAy = startY + 1;
					pathAEnteredFrom = 'N';
					currentPath = 'B';
				}
				else if (currentPath == 'B')
				{
					pathBx = startX;
					pathBy = startY + 1;
					pathBEnteredFrom = 'N';
					currentPath = '?';
				}
			}

			if (possiblePipesToW.Contains(pipeToW))
			{
				// we should already have a path A at this point
				if (currentPath == 'B')
				{
					pathBx = startX - 1;
					pathBy = startY;
					pathBEnteredFrom = 'E';
					currentPath = '?';
				}
			}

			pathAStartX = pathAx;
			pathAStartY = pathAy;
			pathAStartEnteredFrom = pathAEnteredFrom;

			pathBStartX = pathBx;
			pathBStartY = pathBy;
			pathBStartEnteredFrom = pathBEnteredFrom;

			map[pathAy][pathAx] = fullMap[pathAy][pathAx];
			map[pathBy][pathBx] = fullMap[pathBy][pathBx];

			while (true)
			{
				// handle path A

				// handle path B

				// populate the simplified map
				map[pathAy][pathAx] = fullMap[pathAy][pathAx];
				map[pathBy][pathBx] = fullMap[pathBy][pathBx];

				// once the two paths meet up, exit
				if (pathAx == pathBx && pathAy == pathBy)
					break;
			}

			return map;
		}

        private static void PartA()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");
        }

        private static void PartB()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");
        }
    }
}
