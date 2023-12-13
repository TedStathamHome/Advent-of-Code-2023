using System;
using System.IO;
using System.Linq;

namespace Day09
{
    internal class Program
    {
        private static List<OasisReading> OasisReadings;

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023: Day 9");
            var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();
            OasisReadings = ParsePuzzleInput(puzzleInputRaw);

            Console.WriteLine($"* Read in {OasisReadings.Count:N0} lines of OASIS readings.");

            PartA();
            PartB();
        }

        private static List<OasisReading> ParsePuzzleInput(List<string> puzzleInputRaw)
        {
            var oasisReadings = new List<OasisReading>();

            foreach (var puzzleInput in puzzleInputRaw)
            {
                var initialReading = new OasisReading()
                {
                    Readings = new List<List<long>>
                    {
                        puzzleInput.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToList()
                    }
                };

                oasisReadings.Add(initialReading);
            }

            return oasisReadings;
        }

        private static void PartA()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            // build out the sequences
            for (int r = 0; r < OasisReadings.Count; r++)
            {
                var previousSequence = 0;

                Console.WriteLine($"\r\n** Reading {r:N0} - {string.Join(", ", OasisReadings[r].Readings[previousSequence])}");

                while (true)
                {
                    var nextSequence = new List<long>();

                    for (int v = 0; v < OasisReadings[r].Readings[previousSequence].Count - 1; v++)
                    {
                        nextSequence.Add(OasisReadings[r].Readings[previousSequence][v + 1] - OasisReadings[r].Readings[previousSequence][v]);
                    }

                    OasisReadings[r].Readings.Add(nextSequence);
                    previousSequence++;

                    Console.WriteLine($"*** Next sequence {string.Join(", ", nextSequence)}");

                    if (nextSequence.Where(v => v == (long) 0).Count() == nextSequence.Count)
                        break;
                }

            }

            // extrapolate the next values
            ulong sumOfExtrapolatedValues = 0;

            for (int r = 0; r < OasisReadings.Count; r++)
            {
                Console.WriteLine($"\r\n** Extrapolating reading {r:N0}");

                OasisReadings[r].Readings[OasisReadings[r].Readings.Count - 1].Add(0);

                for (int s = OasisReadings[r].Readings.Count - 1; s > 0; s--)
                {
                    var extrapolatedValue = OasisReadings[r].Readings[s][^1] + OasisReadings[r].Readings[s - 1][^1];
                    OasisReadings[r].Readings[s - 1].Add(extrapolatedValue);
                    Console.WriteLine($"*** Extrapolated: {string.Join(", ", OasisReadings[r].Readings[s])}");
                }

                Console.WriteLine($"*** Extrapolated: {string.Join(", ", OasisReadings[r].Readings[0])}");
                sumOfExtrapolatedValues += (ulong) OasisReadings[r].Readings[0][^1];
            }

            Console.WriteLine($"** Sum of extrapolated top-level values: {sumOfExtrapolatedValues:N0}");
        }

        private static void PartB()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");
        }
    }

    internal class OasisReading
    {
        public OasisReading()
        {
            
        }

        public List<List<long>> Readings { get; set; }
    }
}
