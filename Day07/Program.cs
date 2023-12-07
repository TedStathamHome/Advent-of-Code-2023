using System;
using System.IO;
using System.Linq;

namespace Day07
{
	internal class Program
	{
		const string CardsToTranslate = "TJQKA";
		const string TranslatedCardsForSort = "BCDEF";

		const int FiveOfAKind = 6;
		const int FourOfAKind = 5;
		const int FullHouse = 4;
		const int ThreeOfAKind = 3;
		const int TwoPair = 2;
		const int OnePair = 1;
		const int HighCard = 0;

		static void Main(string[] args)
		{
			Console.WriteLine("Advent of Code 2023: Day 7");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

			var puzzleInput = puzzleInputRaw.Select(x => new CamelCardsHand(x)).ToList();

			PartA(puzzleInput);
			PartB();
		}

		private static void PartA(List<CamelCardsHand> puzzleInput)
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part A");

			var rankedHands = puzzleInput.OrderBy(h => h.HandType).ThenBy(h => h.SortedHand).ToList();
			ulong totalWinnings = 0;

			for (int i = 0; i < rankedHands.Count; i++)
			{
				Console.WriteLine($"** Rank {i + 1:N0} - {rankedHands[i].Hand} - Type: {rankedHands[i].HandType:N0} - Won: {(i + 1) * rankedHands[i].Bid:N0}");
				totalWinnings += (i + 1) * rankedHands[i].Bid;
			}

			Console.WriteLine($"*** Total winnings: {totalWinnings:N0}");
		}

		private static void PartB()
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part B");
		}
	}

	internal class CamelCardsHand
	{
		const string LegalCards = "23456789TJQKA";
		const string CardsToTranslate = "TJQKA";
		const string TranslatedCardsForSort = "BCDEF";
		const string TranslatedCardsForSortPartB = "B0DEF";

		const int FiveOfAKind = 6;
		const int FourOfAKind = 5;
		const int FullHouse = 4;
		const int ThreeOfAKind = 3;
		const int TwoPair = 2;
		const int OnePair = 1;
		const int HighCard = 0;

		public CamelCardsHand(string rawHandDetails)
		{
			var details = rawHandDetails.Split(' ').ToList();
			Hand = details[0];
			Bid = ulong.Parse(details[1]);

			foreach (var card in Hand)
			{
				if (!LegalCards.Contains(card))
				{
					Console.WriteLine($"!!! Hand {Hand} contains an illegal card: {card}");
				}
			}

			// since the last 5 card identifiers are not in a sortable order,
			// convert them into characters which can be sorted for later ranking
			var sortedHand = Hand;

			for (int i = 0; i < CardsToTranslate.Length; i++)
			{
				sortedHand = sortedHand.Replace(CardsToTranslate[i], TranslatedCardsForSort[i]);
			}

			SortedHand = sortedHand;

			// like above, but treating J as a Joker, which is the lowest ranked card
			var sortedHandPartB = Hand;

			for (int i = 0; i < CardsToTranslate.Length; i++)
			{
				sortedHandPartB = sortedHandPartB.Replace(CardsToTranslate[i], TranslatedCardsForSortPartB[i]);
			}

			SortedHandPartB = sortedHandPartB;

			// group the individual cards so that we can figure out what
			// type of hand we have for later ranking
			var cardGroups = Hand.ToArray().ToList()
				.GroupBy(c => c)
				.Select(g => new { Card = g.Key, CardCount = g.Count() })
				.OrderByDescending(g => g.CardCount)
				.ToList();

			// determine hand type for Part A
			if (cardGroups.Count == 1)
			{
				// a single group indicates a five of a kind
				HandType = FiveOfAKind;
			}
			else if (cardGroups.Count == 2)
			{
				// two groups indicates either a four of a kind or full house
				if (cardGroups[0].CardCount == 4)
				{
					// The first group has 4 cards in it, so it's a four of a kind
					HandType = FourOfAKind;
				}
				else
				{
					// the first group has 3 cards in it, meaning the second group must have two cards, so it's a full house
					HandType = FullHouse;
				}
			}
			else if (cardGroups.Count == 3)
			{
				// three groups can indicate a three of a kind or two pair
				if (cardGroups[0].CardCount == 3)
				{
					// we have a three of a kind
					HandType = ThreeOfAKind;
				}
				else
				{
					// we have two pairs
					HandType = TwoPair;
				}
			}
			else if (cardGroups.Count == 4)
			{
				// we have a single pair
				HandType = OnePair;
			}
			else 
			{
				// all that's left is high card
				HandType = HighCard;
			}

			// for part B, start of with the same hand type of part A
			HandTypePartB = HandType;
			
			// then update it if the hand contains a Joker (J)
			if (Hand.Contains('J'))
			{
				var numberOfJokers = cardGroups.Where(g => g.Card == 'J').First().CardCount;
				var jokerIndex = cardGroups

			}

			Console.WriteLine($"* Hand {Hand}, bid {Bid:N0}, hand type {HandType:N0}, sorted hand: {SortedHand}");
		}

		public string Hand { get; set; }
		public string HandPartB { get; set; }
		public ulong Bid { get; set; }
		public int HandType { get; set; }
		public int HandTypePartB { get; set; }
		public string SortedHand { get; set; }
		public string SortedHandPartB { get; set; }
	}
}