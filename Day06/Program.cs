using System;
using System.IO;
using System.Linq;

namespace Day06
{
    internal class Program
    {
        private static List<int> Times = new List<int>();
        private static List<int> Distances = new List<int>();

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023: Day 6");
            var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

            Times = puzzleInputRaw[0].Replace("Time: ", "").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(t => int.Parse(t)).ToList();
            Distances = puzzleInputRaw[1].Replace("Distance: ", "").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(d => int.Parse(d)).ToList();

            Console.WriteLine($"* Races to check: {Times.Count:N0}");

            PartA();
            PartB(puzzleInputRaw);
        }

        private static void PartA()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            ulong productOfRecordBreakers = 1;

            for (int i = 0; i < Times.Count; i++)
            {
                ulong recordBreakingWays = 0;

                for (int j = 1; j < Times[i]; j++)
                {
                    var distance = j * (Times[i] - j);
                    if (distance > Distances[i])
                        recordBreakingWays++;
                }

                Console.WriteLine($"** Race {i + 1:N0}: Record breaking ways: {recordBreakingWays:N0}");

                if (recordBreakingWays > 0)
                    productOfRecordBreakers *= recordBreakingWays;
            }

            Console.WriteLine($"** Product of record breaking ways: {productOfRecordBreakers:N0}");
        }

        private static void PartB(List<string> puzzleInputRaw)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

            ulong time = ulong.Parse(puzzleInputRaw[0].Replace("Time: ", "").Replace(" ", ""));
            ulong distance = ulong.Parse(puzzleInputRaw[1].Replace("Distance: ", "").Replace(" ", ""));

            Console.WriteLine($"** Race time: {time:N0}; distance: {distance:N0}");

            ulong recordBreakingWays = 0;

            for (ulong j = 1; j < time; j++)
            {
                var attemptDistance = j * (time - j);
                if (attemptDistance > distance)
                    recordBreakingWays++;
            }

            Console.WriteLine($"** Record breaking ways: {recordBreakingWays:N0}");
        }
    }
}
