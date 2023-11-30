using System;
using System.IO;
using System.Linq;

namespace Day12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023: Day 12");
            var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

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
    }
}
