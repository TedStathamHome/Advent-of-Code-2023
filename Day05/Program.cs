using System;
using System.IO;
using System.Linq;

namespace Day05
{
    internal class Program
    {
        private static List<long>? seeds = new List<long>();
        private static List<ValueTransformation> seedToSoilMappings = new List<ValueTransformation>();
        private static List<ValueTransformation> soilToFertilizerMappings = new List<ValueTransformation>();
        private static List<ValueTransformation> fertilizerToWaterMappings = new List<ValueTransformation>();
        private static List<ValueTransformation> waterToLightMappings = new List<ValueTransformation>();
        private static List<ValueTransformation> lightToTemperatureMappings = new List<ValueTransformation>();
        private static List<ValueTransformation> temperatureToHumidityMappings = new List<ValueTransformation>();
        private static List<ValueTransformation> humidityToLocationMappings = new List<ValueTransformation>();
        private static long absoluteNearestLocation = long.MaxValue;

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023: Day 5");
            var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

            ParsePuzzleInput(puzzleInputRaw);

            PartA();
            PartB();
        }

        private static void ParsePuzzleInput(List<string> puzzleInputRaw)
        {
            var currentLine = 0;

            seeds = puzzleInputRaw[currentLine].Replace("seeds: ", "").Split(' ').Select(s => long.Parse(s)).ToList();
            Console.WriteLine($"* Seeds: {seeds.Count:N0}");
            
            currentLine += 3;

            // seed-to-soil map
            while (puzzleInputRaw[currentLine].Length > 0)
            {
                var lineValues = puzzleInputRaw[currentLine].Split(' ').Select(l => long.Parse(l)).ToList();
                
                seedToSoilMappings.Add(new ValueTransformation()
                {
                    DestinationRangeStart = lineValues[0],
                    SourceRangeStart = lineValues[1],
                    RangeLength = lineValues[2]
                });

                currentLine++;
            }

            Console.WriteLine($"* Seed to soil mappings: {seedToSoilMappings.Count:N0}");

            currentLine += 2;

            // soil-to-fertilizer map
            while (puzzleInputRaw[currentLine].Length > 0)
            {
                var lineValues = puzzleInputRaw[currentLine].Split(' ').Select(l => long.Parse(l)).ToList();

                soilToFertilizerMappings.Add(new ValueTransformation()
                {
                    DestinationRangeStart = lineValues[0],
                    SourceRangeStart = lineValues[1],
                    RangeLength = lineValues[2]
                });

                currentLine++;
            }

            Console.WriteLine($"* Soil to fertilizer mappings: {soilToFertilizerMappings.Count:N0}");

            currentLine += 2;

            // fertilizer-to-water map
            while (puzzleInputRaw[currentLine].Length > 0)
            {
                var lineValues = puzzleInputRaw[currentLine].Split(' ').Select(l => long.Parse(l)).ToList();

                fertilizerToWaterMappings.Add(new ValueTransformation()
                {
                    DestinationRangeStart = lineValues[0],
                    SourceRangeStart = lineValues[1],
                    RangeLength = lineValues[2]
                });

                currentLine++;
            }

            Console.WriteLine($"* Fertilizer to water mappings: {fertilizerToWaterMappings.Count:N0}");

            currentLine += 2;

            // water-to-light map
            while (puzzleInputRaw[currentLine].Length > 0)
            {
                var lineValues = puzzleInputRaw[currentLine].Split(' ').Select(l => long.Parse(l)).ToList();

                waterToLightMappings.Add(new ValueTransformation()
                {
                    DestinationRangeStart = lineValues[0],
                    SourceRangeStart = lineValues[1],
                    RangeLength = lineValues[2]
                });

                currentLine++;
            }

            Console.WriteLine($"* Water to light mappings: {waterToLightMappings.Count:N0}");

            currentLine += 2;

            // light-to-temperature map
            while (puzzleInputRaw[currentLine].Length > 0)
            {
                var lineValues = puzzleInputRaw[currentLine].Split(' ').Select(l => long.Parse(l)).ToList();

                lightToTemperatureMappings.Add(new ValueTransformation()
                {
                    DestinationRangeStart = lineValues[0],
                    SourceRangeStart = lineValues[1],
                    RangeLength = lineValues[2]
                });

                currentLine++;
            }

            Console.WriteLine($"* Light to temperature mappings: {lightToTemperatureMappings.Count:N0}");

            currentLine += 2;

            // temperature-to-humidity map
            while (puzzleInputRaw[currentLine].Length > 0)
            {
                var lineValues = puzzleInputRaw[currentLine].Split(' ').Select(l => long.Parse(l)).ToList();

                temperatureToHumidityMappings.Add(new ValueTransformation()
                {
                    DestinationRangeStart = lineValues[0],
                    SourceRangeStart = lineValues[1],
                    RangeLength = lineValues[2]
                });

                currentLine++;
            }

            Console.WriteLine($"* Temperature to humidity mappings: {temperatureToHumidityMappings.Count:N0}");

            currentLine += 2;

            // humidity-to-location map
            while (currentLine < puzzleInputRaw.Count)
            {
                var lineValues = puzzleInputRaw[currentLine].Split(' ').Select(l => long.Parse(l)).ToList();

                var hToL = new ValueTransformation()
                {
                    DestinationRangeStart = lineValues[0],
                    SourceRangeStart = lineValues[1],
                    RangeLength = lineValues[2]
                };

                humidityToLocationMappings.Add(hToL);

                absoluteNearestLocation = hToL.DestinationRangeStart < absoluteNearestLocation ? hToL.DestinationRangeStart : absoluteNearestLocation;

                currentLine++;
            }

            Console.WriteLine($"* Humidity to location mappings: {humidityToLocationMappings.Count:N0}");
        }

        private static long GetMappedValueForSource(long source, List<ValueTransformation> mappings)
        {
            var mappedValue = source;
            var matches = mappings.Where(m => m.IsSourceInRange(source)).Select(m => m.TransformSourceToDestination(source)).ToList();
            if (matches.Any())
            {
                mappedValue = matches[0];
            }

            return mappedValue;
        }

        private static void PartA()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            var locations = new List<long>();

            foreach (var seed in seeds)
            {
                var soil = GetMappedValueForSource(seed, seedToSoilMappings);
                var fertilizer = GetMappedValueForSource(soil, soilToFertilizerMappings);
                var water = GetMappedValueForSource(fertilizer, fertilizerToWaterMappings);
                var light = GetMappedValueForSource(water, waterToLightMappings);
                var temperature = GetMappedValueForSource(light, lightToTemperatureMappings);
                var humidity = GetMappedValueForSource(temperature, temperatureToHumidityMappings);
                var location = GetMappedValueForSource(humidity, humidityToLocationMappings);

                Console.WriteLine($"** Seed: {seed:N0} => Soil: {soil:N0} => Fertilizer: {fertilizer:N0} => Water: {water:N0} => Light: {light:N0} => Temperature: {temperature:N0} => Humidity: {humidity:N0} => Location: {location:N0}");

                locations.Add(location);
            }

            Console.WriteLine($"** Nearest location: {locations.Min():N0}");
        }

        private static void PartB()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

            long seedsChecked = 0;
            long nearestLocation = long.MaxValue;
            bool haveFoundAbsoluteNearestLocation = false;

            for (var i = 0; i < seeds.Count; i += 2)
            {
                for (var j = seeds[i]; j < seeds[i] + seeds[i + 1]; j++)
                {
                    seedsChecked++;

                    var soil = GetMappedValueForSource(j, seedToSoilMappings);
                    var fertilizer = GetMappedValueForSource(soil, soilToFertilizerMappings);
                    var water = GetMappedValueForSource(fertilizer, fertilizerToWaterMappings);
                    var light = GetMappedValueForSource(water, waterToLightMappings);
                    var temperature = GetMappedValueForSource(light, lightToTemperatureMappings);
                    var humidity = GetMappedValueForSource(temperature, temperatureToHumidityMappings);
                    var location = GetMappedValueForSource(humidity, humidityToLocationMappings);

                    nearestLocation = (location < nearestLocation) ? location : nearestLocation;

                    haveFoundAbsoluteNearestLocation = nearestLocation == absoluteNearestLocation;

                    if (haveFoundAbsoluteNearestLocation) { break; }
                }

                if (haveFoundAbsoluteNearestLocation) { break; }
            }

            Console.WriteLine($"** Count of seeds checked: {seedsChecked:N0}");
            Console.WriteLine($"** Exited early for hitting absolute nearest location? {haveFoundAbsoluteNearestLocation}");
            Console.WriteLine($"** Nearest location: {nearestLocation:N0}");
        }
    }

    internal class ValueTransformation
    {
        public ValueTransformation()
        {
            
        }

        public long DestinationRangeStart { get; set; }

        public long SourceRangeStart { get; set; }

        public long RangeLength { get; set; }

        public long RangeStartOffset 
            => DestinationRangeStart - SourceRangeStart;
        
        public long SourceRangeEnd 
            => SourceRangeStart + RangeLength - 1;

        public bool IsSourceInRange(long source)
        {
            return (source >= SourceRangeStart && source <= SourceRangeEnd);
        }

        public long TransformSourceToDestination(long source)
        {
            return IsSourceInRange(source) ? (source + RangeStartOffset) : source;
        }

    }
}
