using System;
using System.IO;
using System.Linq;

namespace Day01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023: Day 1");
            var puzzleInputRawA = File.ReadLines($"./PuzzleInput-test-PartA.txt").ToList();
			var puzzleInputRawB = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();


			// needed for Part A
			var calibrationValueDigitsA = puzzleInputRawA.Select(x => string.Join("", x.Select(y => "0123456789".Contains(y) ? y.ToString() : "").ToList())).ToList();

			// needed for Part B
			var convertedInput = ConvertNumberWordsToDigits(puzzleInputRawB);
			var calibrationValueDigitsB = convertedInput.Select(x => string.Join("", x.Select(y => "0123456789".Contains(y) ? y.ToString() : "").ToList())).ToList();

			//foreach( var calibrationValue in calibrationValueDigits)
			//{
			//	Console.WriteLine($"** {calibrationValue}");
			//}

			PartA(calibrationValueDigitsA);
            PartB(calibrationValueDigitsB);
        }

		private static List<string>? ConvertNumberWordsToDigits(List<string>? puzzleInputRaw)
		{
			var numberWords = "zero,one,two,three,four,five,six,seven,eight,nine".Split(",").ToList();
			var convertedInput = new List<string>();

			foreach (string puzzleInput in puzzleInputRaw)
			{
				var convertedValue = "";
				var currentChar = 0;
				var foundWord = false;
				var atEndOfWord = false;

				while (currentChar < puzzleInput.Length)
				{
					var textToCheck = puzzleInput.Substring(currentChar);
					foundWord = false;

					for (var i = 0; i < numberWords.Count; i++)
					{
						if (textToCheck.StartsWith(numberWords[i]))
						{
							convertedValue += $"{i:N0}";
							currentChar += numberWords[i].Length - 1;
							foundWord = true;
							atEndOfWord = true;
							break;
						}
					}

					if (!foundWord)
					{
						currentChar++;
						if (atEndOfWord)
						{
							atEndOfWord = false;
						}
						else
						{
							convertedValue += textToCheck[0];
						}
					}
				}
				
				convertedInput.Add(convertedValue);
				//Console.WriteLine($"** Original value:  {puzzleInput}");
				//Console.WriteLine($"** Converted value: {convertedValue}\r\n");
			}

			return convertedInput;
		}

        private static void PartA(List<string>? calibrationValueDigits)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

			Console.WriteLine($"* Read in {calibrationValueDigits.Count:N0} messed up calibration values");

			int sumOfValues = 0;
			foreach (var calibrationValue in calibrationValueDigits)
			{
				var digits = calibrationValue[0].ToString() + calibrationValue[^1].ToString();
				Console.WriteLine($"** {digits}");
				sumOfValues += int.Parse(digits);
			}

			Console.WriteLine($"*** Sum of calibration values: {sumOfValues:N0}");
        }

        private static void PartB(List<string>? calibrationValueDigits)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

			Console.WriteLine($"* Read in {calibrationValueDigits.Count:N0} messed up calibration values");

			int sumOfValues = 0;
			foreach (var calibrationValue in calibrationValueDigits)
			{
				var digits = calibrationValue[0].ToString() + calibrationValue[^1].ToString();
				Console.WriteLine($"** {digits}");
				sumOfValues += int.Parse(digits);
			}

			Console.WriteLine($"*** Sum of calibration values: {sumOfValues:N0}");
		}
	}
}
