using System;
using System.IO;
using System.Linq;

namespace Day11
{
    internal class Program
    {
		private const string EmptySpace = ".";
		private const string Galaxy = "#";

		static void Main(string[] args)
		{
			Console.WriteLine("Advent of Code 2023: Day 11");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();
			var expandedUniverse = ExpandTheUniverse(puzzleInputRaw);
			var galaxyCoordinates = FindTheGalaxies(expandedUniverse);

			Console.WriteLine($"Original universe is {puzzleInputRaw[0].Length:N0} x {puzzleInputRaw.Count:N0}");
			Console.WriteLine($"Number of galaxies: {galaxyCoordinates.Count:N0}");
			Console.WriteLine($"Expanded universe is {expandedUniverse[0].Length:N0} x {expandedUniverse.Count:N0}");

			for (int i = 0; i < expandedUniverse.Count; i++)
			{
				Console.WriteLine(expandedUniverse[i]);
			}

			PartA();
			PartB();
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

		private static List<string> ExpandTheUniverse(List<string> originalUniverse)
		{
			var expandedUniverse = originalUniverse.ToList();

			// expand the universe vertically, from the bottom up
			for (var i = expandedUniverse.Count - 1; i >= 0; i--)
			{
				if (expandedUniverse[i].Replace(EmptySpace, "").Length == 0)
				{
					expandedUniverse.Insert(i, expandedUniverse[i]);
				}
			}

			// expand the universe horizontally, from right to left
			for (var i = expandedUniverse[0].Length - 1; i >= 0; i--)
			{
				var galaxyColumn = string.Join("", expandedUniverse.Select(x => x[i]).ToList());
				
				if (galaxyColumn.Replace(EmptySpace, "").Length == 0)
				{
					for (var j = 0; j < expandedUniverse.Count; j++)
					{
						expandedUniverse[j] = expandedUniverse[j].Insert(i, EmptySpace);
					}
				}
			}

			return expandedUniverse;
		}

		private static List<GalaxyCoordinate> FindTheGalaxies(List<string> universe)
		{
			var galaxyCoordinates = new List<GalaxyCoordinate>();

			for (var r = 0; r < universe.Count; r++)
			{
				if (universe[r].Replace(EmptySpace, "").Length > 0)
				{
					var c = universe[r].IndexOf(Galaxy);

					while (c >= 0)
					{
						galaxyCoordinates.Add(new GalaxyCoordinate() { Row = r, Column = c });
						c = universe[r].IndexOf(Galaxy, c + 1);
					}
				}
			}

			return galaxyCoordinates;
		}
    }

	internal class GalaxyCoordinate
	{
		public int Row { get; set; }
		public int Column { get; set; }
	}
}
