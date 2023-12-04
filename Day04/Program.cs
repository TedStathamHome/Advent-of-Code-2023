using System;
using System.IO;
using System.Linq;

namespace Day04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023: Day 4");
            var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

            var cardDetails = ParsePuzzleInput(puzzleInputRaw);

            Console.WriteLine($"* Cards to check: {cardDetails.Count:N0}");

            PartA(cardDetails);
            PartB(cardDetails);
        }

        private static List<CardDetail> ParsePuzzleInput(List<string> puzzleInputRaw)
        {
            var cardInfo = puzzleInputRaw.Select(x => x.Replace("Card ", "").Split(":|".ToCharArray(), StringSplitOptions.TrimEntries)).ToList();
            var cardDetails = new List<CardDetail>();
            
            foreach (var card in cardInfo)
            {
                var cardDetail = new CardDetail()
                {
                    CardNo = int.Parse(card[0]),
                    WinningNumbers = card[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).Order().ToList(),
                    NumbersOnCard = card[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).Order().ToList()
                };

                var matches = cardDetail.WinningNumbers.Intersect(cardDetail.NumbersOnCard).ToList().Count();
                if (matches > 0)
                {
                    var ticketsWon = new List<int>();
                    
                    for (int i = 0; i < matches; i++)
                    {
                        ticketsWon.Add(cardDetail.CardNo + i + 1);
                    }

                    cardDetail.TicketsWon = ticketsWon.ToList();
                }

                cardDetails.Add(cardDetail);
            }

            return cardDetails;
        }

        private static void PartA(List<CardDetail> cardDetails)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            var cardResults = cardDetails.Select(x => x.WinningNumbers.Intersect(x.NumbersOnCard).ToList().Count).ToList();
            var totalPoints = cardResults.Select(x => x).Where(x => x > 0).Sum(x => Math.Pow(2, x - 1));

            Console.WriteLine($"** Total points: {totalPoints:N0}");
        }

        private static void PartB(List<CardDetail> cardDetails)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

            var cardsPlayed = cardDetails.Select(x => x.CardNo - 1).ToList();
            var currentCard = 0;

            while (currentCard < cardsPlayed.Count)
            {
                if (cardDetails[cardsPlayed[currentCard]].TicketsWon.Count > 0)
                {
                    cardsPlayed.AddRange(cardDetails[cardsPlayed[currentCard]].TicketsWon.Select(x => x - 1));
                }

                currentCard++;
            }

            Console.WriteLine($"** Total cards played: {cardsPlayed.Count:N0}");
        }
    }

    internal class CardDetail
    {
        public CardDetail()
        {
            WinningNumbers = new List<int>();
            NumbersOnCard = new List<int>();
            TicketsWon = new List<int>();
        }

        public int CardNo { get; set; }

        public List<int> WinningNumbers { get; set; }

        public List<int> NumbersOnCard { get; set; }

        public List<int> TicketsWon {  get; set; }
    }
}
