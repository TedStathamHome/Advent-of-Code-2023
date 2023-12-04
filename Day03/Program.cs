using System;
using System.IO;
using System.Linq;

namespace Day03
{
    internal class Program
    {
        private const string digits = "0123456789";
        private const string nonSymbols = ".0123456789";

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023: Day 3");
            var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

            Console.WriteLine($"Schematic is {puzzleInputRaw[0].Length:N0} x {puzzleInputRaw.Count:N0}");

            PartA(puzzleInputRaw);
            PartB(puzzleInputRaw);
        }

        private static void PartA(List<string> schematic)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            var foundNumbers = new List<int>();
            var rowCount = schematic.Count;
            var columnsInRow = schematic[0].Length;
            var symbols = "@#$%&*-=+/?";

            for (int row = 0; row < rowCount; row++)
            {
                var rowValues = schematic[row].Split((symbols + ".").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                
                if (rowValues.Length > 0)
                {
                    var currentCol = 0;

                    foreach (var value in rowValues)
                    {
                        var startCol = schematic[row].IndexOf(value, currentCol);
                        var endCol = startCol + value.Length - 1;
                        var foundSymbolNearNumber = false;
                        
                        for (int checkRow = row - 1; checkRow < row + 2; checkRow++)
                        {
                            if (checkRow >= 0 && checkRow < rowCount) 
                            {
                                for (int checkCol = startCol - 1; checkCol < endCol + 2; checkCol++)
                                {
                                    if (checkCol >= 0 && checkCol < columnsInRow)
                                    {
                                        var charToCheck = schematic[checkRow][checkCol];

                                        if (checkRow == row)
                                        {
                                            // skip looking through the number itself
                                            if (checkCol < startCol || checkCol > endCol)
                                            {
                                                foundSymbolNearNumber = symbols.Contains(charToCheck);
                                            }
                                        }
                                        else
                                        {
                                            foundSymbolNearNumber = symbols.Contains(charToCheck);
                                        }

                                        if (foundSymbolNearNumber) { break; }
                                    }
                                }

                                if (foundSymbolNearNumber) { break; }
                            }
                        }

                        if (foundSymbolNearNumber)
                        {
                            foundNumbers.Add(int.Parse(value));
                            Console.WriteLine($"*** Row {row:N0} >> Found {value}");
                        }
                        else
                        {
                            Console.WriteLine($"!!! Row {row:N0} >> {value} not near any symbols");
                        }

                        currentCol = endCol + 1;
                    }
                }
            }

            Console.WriteLine($"** Found {foundNumbers.Count:N0} part number(s) near symbols");
            Console.WriteLine($"** Sum of part numbers: {foundNumbers.Sum():N0}");
        }

        private static void PartB(List<string> schematic)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

            var gearCoordinates = new List<(int, int)>();

            // find the gears
            for (int row = 0; row < schematic.Count; row++)
            {
                var col = -1;
                var nextGear = schematic[row].IndexOf('*', col + 1);

                while (nextGear >= 0)
                {
                    Console.WriteLine($"** Gear at {row}, {nextGear}");

                    gearCoordinates.Add((row, nextGear));
                    col = nextGear;
                    nextGear = schematic[row].IndexOf('*', col + 1);
                }
            }

            var rowCount = schematic.Count;
            var columnsInRow = schematic[0].Length;
            long sumOfGearRatios = 0;

            foreach (var gearCoordinate in gearCoordinates)
            {
                var (row, col) = gearCoordinate;
                var nearbyParts = new List<int>();
                
                for (var checkRow = row - 1; checkRow < (row + 2); checkRow++)
                {
                    if (checkRow >= 0 && checkRow < rowCount)
                    {
                        for (var checkCol = col - 1; checkCol < (col + 2); checkCol++)
                        {
                            // if we're not outside of bounds and we're not looking at the gear itself
                            if (checkCol >= 0 && checkCol < columnsInRow && !(checkRow == row && checkCol == col))
                            {
                                if (digits.Contains(schematic[checkRow][checkCol]))
                                {
                                    var partNumber = schematic[checkRow][checkCol].ToString();
                                    var maxCol = checkCol;

                                    // gather up digits to the left
                                    for (var i = 1; i < 5; i++)
                                    {
                                        if (checkCol - i < 0)
                                            break;

                                        if (!digits.Contains(schematic[checkRow][checkCol - i]))
                                            break;

                                        partNumber = schematic[checkRow][checkCol - i].ToString() + partNumber;
                                    }

                                    // gather up digits to the right
                                    for (var i = 1; i < 5; i++)
                                    {
                                        if (checkCol + i >= columnsInRow)
                                            break;

                                        if (!digits.Contains(schematic[checkRow][checkCol + i]))
                                            break;

                                        partNumber += schematic[checkRow][checkCol + i].ToString();
                                        maxCol = checkCol + i;
                                    }

                                    // force the next iteration to look past the end of the number found
                                    checkCol = maxCol;
                                    nearbyParts.Add(int.Parse(partNumber));
                                }
                            }
                        }
                    }
                }

                Console.WriteLine($"*** Gear at {row}, {col}: Part(s) - {string.Join(", ", nearbyParts.Select(pn => pn.ToString()).ToList())}");

                if (nearbyParts.Count == 2)
                {
                    sumOfGearRatios += nearbyParts[0] * nearbyParts[1];
                }
            }

            Console.WriteLine($"** Sum of gear ratios: {sumOfGearRatios:N0}");
        }
    }
}
