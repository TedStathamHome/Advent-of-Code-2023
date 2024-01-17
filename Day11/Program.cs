using System;
using System.IO;
using System.Linq;

namespace Day11
{
    internal class Program
    {
		private const string EmptySpace = ".";
		private const string Galaxy = "#";
		// private const int ExpansionRate = 2;
		// private const int ExpansionRate = 100;
		private const int ExpansionRate = 1_000_000;
		private static List<int> ColsToExpand;
		private	static List<int> RowsToExpand;

		static void Main(string[] args)
		{
			Console.WriteLine("Advent of Code 2023: Day 11");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();
			ColsToExpand = FindEmptyColumns(puzzleInputRaw);
			RowsToExpand = FindEmptyRows(puzzleInputRaw);
			var galaxyCoordinates = FindTheGalaxies(puzzleInputRaw);
			var galaxyPairs = BuildGalaxyPairs(galaxyCoordinates);

			Console.WriteLine($"Original universe is {puzzleInputRaw[0].Length:N0} x {puzzleInputRaw.Count:N0}");
			Console.WriteLine($"Expanded universe is {puzzleInputRaw[0].Length + (ColsToExpand.Count * (ExpansionRate - 1)):N0} x {puzzleInputRaw.Count + (RowsToExpand.Count * (ExpansionRate - 1)):N0}");
			Console.WriteLine($"Number of galaxies: {galaxyCoordinates.Count:N0}");
			
			PartA(galaxyPairs);
		}

		private static List<int> FindEmptyColumns(List<string> universe)
		{
			var emptyCols = new List<int>();

			for (var c = 0; c < universe[0].Length; c++)
			{
				var universeColumn = string.Join("", universe.Select(l => l[c]).ToList());

				if (universeColumn.Replace(EmptySpace, "").Length == 0)
				{
					emptyCols.Add(c);
				}
			}

			return emptyCols;
		}

		private static List<int> FindEmptyRows(List<string> universe)
		{
			var emptyRows = new List<int>();

			for (var r = 0; r < universe.Count; r++)
			{
				if (universe[r].Replace(EmptySpace, "").Length == 0)
				{
					emptyRows.Add(r);
				}
			}

			return emptyRows;
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
				Console.WriteLine($"*** {i:N0}: {galaxyPair.GalaxyA} -> {galaxyPair.GalaxyB} = {galaxyPair.PathLength:N0} (iterations: {galaxyPair.Iterations:N0})");
				i++;
			}

			ulong sumOfShortestPaths = 0;
			
			foreach (var galaxyPair in galaxyPairs)
			{
				sumOfShortestPaths += galaxyPair.PathLength;
			}

			//var sumOfShortestPaths = galaxyPairs.Sum(gp => (ulong) gp.PathLength);
			Console.WriteLine($"** Sum of shortest paths: {sumOfShortestPaths:N0}");
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
						galaxyCoordinates.Add(new GalaxyCoordinate()
						{
							Row = r,
							Column = c
						});
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

					galaxyPairs.Add(new GalaxyPair() 
					{
						GalaxyA = i,
						GalaxyB = j,
						PathLength = pathLength.PathLength,
						Iterations = pathLength.Iterations
					});
				}
			}

			return galaxyPairs;
		}

		private static (ulong PathLength, int Iterations) PathLengthBetweenGalaxies(GalaxyCoordinate galaxyA, GalaxyCoordinate galaxyB)
		{
			int emptyColsBetweenGalaxies = 0;
			// if galaxy A is to the left of galaxy B by more than 1 column, look for the empty columns between them
			if (galaxyA.Column < galaxyB.Column && (galaxyB.Column - galaxyA.Column) > 1)
			{
				emptyColsBetweenGalaxies = ColsToExpand.Where(c => c > galaxyA.Column && c < galaxyB.Column).Count();
			}
			// if galaxy A is to the right of galaxy B by more than 1 column, look for the empty columns between them
			else if (galaxyA.Column > galaxyB.Column && (galaxyA.Column - galaxyB.Column) > 1)
			{
				emptyColsBetweenGalaxies = ColsToExpand.Where(c => c > galaxyB.Column && c < galaxyA.Column).Count();
			}

			// this can be zero (same column), positive (to the right) or negative (to the left)
			var xDistance = galaxyB.Column - galaxyA.Column;

			// adjust for the extra space needed for galactic expansion
			xDistance += ((xDistance >= 0) ? 1 : -1) * emptyColsBetweenGalaxies * (ExpansionRate - 1);


			// this will always be zero (same row) or positive (below)
			var yDistance = galaxyB.Row - galaxyA.Row;

			int emptyRowsBetweenGalaxies = 0;
			// if galaxy B is below galaxy A by more than 1 row, look for the empty rows between them
			if (yDistance > 1)
			{
				emptyRowsBetweenGalaxies = RowsToExpand.Where(r => r > galaxyA.Row && r < galaxyB.Row).Count();
			}

			// adjust for the extra space needed for galactic expansion
			yDistance += emptyRowsBetweenGalaxies * (ExpansionRate - 1);


			// if both galaxies are on the same column, the shortest path is a vertical straight line
			if (xDistance == 0)
			{
				return ((ulong)int.Abs(yDistance), 1);
			}

			// if both galaxies are on the same row, the shortest path is a horizontal straight line
			if (yDistance == 0)
			{
				return ((ulong)int.Abs(xDistance), 1);
			}

			ulong pathLength = 0;
			var iterations = 0;

			// loop until there is no more x or y distance remaining
			while (xDistance != 0 || yDistance != 0)
			{
				// pathLength++;
				iterations++;
				var xChange = 0;
				var yChange = 0;

				if (int.Abs(xDistance) == int.Abs(yDistance))
				{
					xChange = -xDistance;
					yChange = -yDistance;
					pathLength += (ulong)int.Abs(xChange) + (ulong)int.Abs(yChange);
				}
				// if we have farther to go (or the same) on the x axis, move on the x axis
				else if (int.Abs(xDistance) > int.Abs(yDistance))
				{
					var relativeDistanceDiff = (int.Abs(xDistance) - int.Abs(yDistance));
					
					// if the x and y distances are the same, move at least one space on the x axis
					if (relativeDistanceDiff == 0)
					{
						xChange = xDistance > 0 ? -1 : 1;
					}
					else
					{
						// otherwise, move as many steps as it takes to make the x and y distances equal
						xChange = (xDistance > 0 ? -1 : 1) * relativeDistanceDiff;
					}

					pathLength += (ulong) int.Abs(xChange);
				}
				else
				{
					// we have farther to go on the y axis
					// this should always be at least 1
					var relativeDistanceDiff = (int.Abs(yDistance) - int.Abs(xDistance));

					// move as many steps as it takes to make the y and x distances equal
					yChange = (yDistance > 0 ? -1 : 1) * relativeDistanceDiff;

					pathLength += (ulong) int.Abs(yChange);
				}

				xDistance += xChange;
				yDistance += yChange;
			}

			return (pathLength, iterations);
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
		public ulong PathLength { get; set; }
		public int Iterations { get; set; }
	}
}
