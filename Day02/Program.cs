using System;
using System.IO;
using System.Linq;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023: Day 2");
            var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();
            Console.WriteLine($"** Games to inspect: {puzzleInputRaw.Count:N0}");

            PartA(puzzleInputRaw);
            PartB(puzzleInputRaw);
        }

        private static void PartA(List<string> puzzleInputRaw)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            var sumOfPossibleGameIDs = 0;
            const int maxBlueCubes = 14;
            const int maxGreenCubes = 13;
            const int maxRedCubes = 12;

            foreach (var puzzleInput in puzzleInputRaw) 
            {
                var gameBaseData = puzzleInput.Split(':', StringSplitOptions.TrimEntries);
                var gameID = int.Parse(gameBaseData[0].Split(' ')[1]);
                var cubeSets = gameBaseData[1].Split(';', StringSplitOptions.TrimEntries);
                var gameIsPossible = true;

                foreach (var cubeSet in cubeSets)
                {
                    var cubes = cubeSet.Split(',', StringSplitOptions.TrimEntries);
                    
                    foreach (var cube in cubes)
                    {
                        var cubeInfo = cube.Split(' ', StringSplitOptions.TrimEntries);
                        var cubeColor = cubeInfo[1];
                        var cubesPicked = int.Parse(cubeInfo[0]);

                        switch (cubeColor)
                        {
                            case "blue":
                                if (cubesPicked > maxBlueCubes)
                                    gameIsPossible = false;
                                break;

                            case "green":
                                if (cubesPicked > maxGreenCubes)
                                    gameIsPossible = false;
                                break;
                            
                            case "red":
                                if (cubesPicked > maxRedCubes)
                                    gameIsPossible = false;
                                break;

                            default:
                                Console.WriteLine($"!! Invalid cube color: '{cubeColor}'");
                                gameIsPossible = false;
                                break;
                        }

                        if (!gameIsPossible)
                            break;
                    }

                    if (!gameIsPossible)
                        break;
                }

                if (gameIsPossible)
                {
                    Console.WriteLine($"** Game {gameID:N0} was possible:");
                    Console.WriteLine($"*** {gameBaseData[1]}\r\n");
                    sumOfPossibleGameIDs += gameID;
                }
                else
                {
                    Console.WriteLine($"!! Game {gameID:N0} was NOT possible!");
                    Console.WriteLine($"!!! {gameBaseData[1]}\r\n");
                }
            }

            Console.WriteLine($"* Sum of possible game IDs: {sumOfPossibleGameIDs:N0}");
        }

        private static void PartB(List<string> puzzleInputRaw)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

            var sumOfPowers = 0;

            foreach (var puzzleInput in puzzleInputRaw)
            {
                var gameBaseData = puzzleInput.Split(':', StringSplitOptions.TrimEntries);
                var gameID = int.Parse(gameBaseData[0].Split(' ')[1]);
                var cubeSets = gameBaseData[1].Split(';', StringSplitOptions.TrimEntries);
                var minBlueCubes = 0;
                var minGreenCubes = 0;
                var minRedCubes = 0;

                foreach (var cubeSet in cubeSets)
                {
                    var cubes = cubeSet.Split(',', StringSplitOptions.TrimEntries);

                    foreach (var cube in cubes)
                    {
                        var cubeInfo = cube.Split(' ', StringSplitOptions.TrimEntries);
                        var cubeColor = cubeInfo[1];
                        var cubesPicked = int.Parse(cubeInfo[0]);

                        switch (cubeColor)
                        {
                            case "blue":
                                if (cubesPicked > minBlueCubes)
                                    minBlueCubes = cubesPicked;
                                break;

                            case "green":
                                if (cubesPicked > minGreenCubes)
                                    minGreenCubes = cubesPicked;
                                break;

                            case "red":
                                if (cubesPicked > minRedCubes)
                                    minRedCubes = cubesPicked;
                                break;

                            default:
                                Console.WriteLine($"!! Invalid cube color: '{cubeColor}'");
                                break;
                        }
                    }
                }

                var powerOfCubeCounts = minBlueCubes * minGreenCubes * minRedCubes;
                sumOfPowers += powerOfCubeCounts;
             
                Console.WriteLine($"** Game {gameID:N0}: min cubes: {minBlueCubes:N0} blue, {minGreenCubes:N0} green, {minRedCubes:N0} red --- Power: {powerOfCubeCounts:N0}");
            }

            Console.WriteLine($"* Sum of powers: {sumOfPowers:N0}");
        }
    }
}
