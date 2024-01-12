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
			var galaxyPairs = BuildGalaxyPairs(galaxyCoordinates);

			Console.WriteLine($"Original universe is {puzzleInputRaw[0].Length:N0} x {puzzleInputRaw.Count:N0}");
			Console.WriteLine($"Number of galaxies: {galaxyCoordinates.Count:N0}");
			Console.WriteLine($"Expanded universe is {expandedUniverse[0].Length:N0} x {expandedUniverse.Count:N0}");

			for (int i = 0; i < expandedUniverse.Count; i++)
			{
				Console.WriteLine(expandedUniverse[i]);
			}

			PartA(galaxyPairs);
			PartB();
		}

        private static void PartA(List<GalaxyPair> galaxyPairs)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");
			Console.WriteLine($"** Number of galaxy pairs: {galaxyPairs.Count:N0}");
			Console.WriteLine("*** Distances between galaxies:");

			var i = 0;

			foreach (var galaxyPair in galaxyPairs)
			{
				Console.WriteLine($"*** {i:N0}: {galaxyPair.GalaxyA} -> {galaxyPair.GalaxyB} = {galaxyPair.PathLength:N0}");
				i++;
			}

			var sumOfShortestPaths = galaxyPairs.Sum(gp => gp.PathLength);
			Console.WriteLine($"** Sum of shortest paths: {sumOfShortestPaths:N0}");
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

		private static List<GalaxyPair> BuildGalaxyPairs(List<GalaxyCoordinate> galaxyCoordinates)
		{
			var galaxyPairs = new List<GalaxyPair>();

			for (var i = 0; i < galaxyCoordinates.Count - 1; i++)
			{
				for (var j = i + 1; j < galaxyCoordinates.Count; j++)
				{
					var pathLength = PathLengthBetweenGalaxies(galaxyCoordinates[i], galaxyCoordinates[j]);

					galaxyPairs.Add(new GalaxyPair() { GalaxyA = i, GalaxyB = j, PathLength = pathLength }); 
				}
			}

			return galaxyPairs;
		}

		private static int PathLengthBetweenGalaxies(GalaxyCoordinate galaxyA, GalaxyCoordinate galaxyB)
		{
			// this can be zero (same column), positive (to the right) or negative (to the left)
			var xDistance = galaxyB.Column - galaxyA.Column;
			
			// this will always be zero (same row) or positive (below)
			var yDistance = galaxyB.Row - galaxyA.Row;

			// if both galaxies are on the same column, the shortest path is a vertical straight line
			if (xDistance == 0) return int.Abs(yDistance);

			// if both galaxies are on the same row, the shortest path is a horizontal straight line
			if (yDistance == 0) return int.Abs(xDistance);

			var pathLength = 0;

			// loop until there is no more x or y distance remaining
			while (xDistance != 0 || yDistance != 0)
			{
				pathLength++;
				var xChange = 0;
				var yChange = 0;

				// if we have farther to go (or the same) on the x axis, move on the x axis
				if (int.Abs(xDistance) >= int.Abs(yDistance))
				{
					xChange = xDistance > 0 ? -1 : 1;
				}
				else
				{
					// we have farther to go on the y axis
					yChange = yDistance > 0 ? -1 : 1;
				}

				xDistance += xChange;
				yDistance += yChange;
			}

			return pathLength;
		}
	}

	internal class GalaxyCoordinate
	{
		public int Row { get; set; }
		public int Column { get; set; }
	}

	internal class GalaxyPair
	{
		public int GalaxyA { get; set; }
		public int GalaxyB { get; set; }
		public int PathLength { get; set; }
	}
}
