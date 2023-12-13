using System;
using System.IO;
using System.Linq;

namespace Day10
{
    internal class Program
    {
		private static int startX;
		private static int startY;
		private static int mapSizeX;
		private static int mapSizeY;

		private const char startPos = 'S';

		private const char vertical = '│';
		private const char horizontal = '─';
		private const char bendNtoE = '└';
		private const char bendNtoW = '┘';
		private const char bendStoW = '┐';
		private const char bendStoE = '┌';
		private const char ground = '░';

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

			var fullMap = puzzleInputRaw.Select(x => x
				.Replace(origVertical, vertical)
				.Replace(origHorizontal, horizontal)
				.Replace(origBendNtoE, bendNtoE)
				.Replace(origBendNtoW, bendNtoW)
				.Replace(origBendStoW, bendStoW)
				.Replace(origBendStoE, bendStoE)
				.Replace(origGround, ground)
				.ToCharArray()).ToList();
			
			for (int i = 0; i < fullMap.Count; i++)
			{
				Console.WriteLine($"{string.Join("", fullMap[i])}");
			}

			var simplifiedMap = SimplifyMap(puzzleInputRaw, fullMap);

            PartA();
            PartB();
        }

		private static List<char[]> SimplifyMap(List<string> puzzleInputRaw, List<char[]> fullMap)
		{
			for (int i = 0; i < puzzleInputRaw.Count; i++)
			{
				var positionOfS = puzzleInputRaw[i].IndexOf(startPos);

				if (positionOfS >= 0)
				{
					startX = positionOfS;
					startY = i;
					break;	// we've found the starting character, so exit early
				}
			}

			var map = new List<char[]>();

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
