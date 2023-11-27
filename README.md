# Advent-of-Code-2023
## My solutions to the [2023 edition](https://adventofcode.com/2023) of the [Advent of Code](https://adventofcode.com/) challenge.

It is a single solution, authored in _Visual Studio Code_ using .NET 7 C# console applications. Each of the 25 days is its own project, containing the C# code, as well as my puzzle input. The solutions will work, but they may not be the most efficient implementations.

Usually there are two or more puzzle input files.
- The ones that end with **"-test"** contain the sample input shown in the puzzle description, and were used to vet out the solution logic before working with the official puzzle input.
- The ones that end with **"-full"** contain the input that was generated specifically for my Advent of Code account. As such, you can't submit an answer for your account using my input, so make sure you use your own puzzle input. And please don't attempt to push your puzzle input into my repo; doing so will get it rejected.

When running each day's code, you can pass in a parameter of `test` to force it to run with the test puzzle input. Not providing a parameter or giving it any other value will make it run with the full puzzle input. For example, on Linux, the following command will run day 3's solution using the test puzzle input:

`dotnet Day03.dll test`
