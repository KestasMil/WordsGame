using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using WordsGameCore;
using WordsGameCore.Exceptions;
using WordsGameCore.Exceptions.WordValidator;
using WordsGameCore.Extensions;
using WordsGameCore.Services;

namespace WordsGameCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            // I included 3 options of how game "engine" can be initialized.

            // Option 1
            // DI - Manually add all game infrastructure to DI container.
            /*IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                services.AddSingleton<HttpClient>()
                    .AddSingleton<IWordValidator, WordValidator>()
                    .AddSingleton<IScheduler, Scheduler>()
                    .AddSingleton<WordsGameEngine>())
                .Build();*/

            // Option 2
            // DI - Use extension method provided by WordsGameCore project which does the same thing as above, but in a single extension method, this is my prefered way.
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                services.UseWordsGameEngineDefaults())
                .Build();

            var wordsGameEngine = host.Services.GetService<WordsGameEngine>();

            // Option 3
            // Manually instantiate the game engine.
            //WordsGameEngine wordsGameEngine = new WordsGameEngine(new WordValidator(new System.Net.Http.HttpClient()), new Scheduler(), 90);

            while (true)
            {
                switch (wordsGameEngine.GameStatus)
                {
                    case GameStatus.NotStarted:
                        HandleNotStarted(wordsGameEngine);
                        break;
                    case GameStatus.InProgress:
                        HandleInProgress(wordsGameEngine);
                        break;
                    case GameStatus.Ended:
                        HandleEnded(wordsGameEngine);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void HandleNotStarted(WordsGameEngine wordsGameEngine)
        {
            Console.WriteLine("Press 'Enter' to start.");
            Console.ReadLine();
            wordsGameEngine.StartGame();
        }

        private static void HandleInProgress(WordsGameEngine wordsGameEngine)
        {
            Console.WriteLine($"Your letters: {string.Join(' ', wordsGameEngine.Letters)} (Time remaining: {wordsGameEngine.GetTimeRemaining()}s)");
            var input = Console.ReadLine();

            if (wordsGameEngine.GameStatus != GameStatus.InProgress)
            {
                Console.WriteLine("\nUnfortunetly you ran out of time :(");
                return;
            }

            try
            {
                if (wordsGameEngine.SubmitWord(input))
                {
                    Console.WriteLine($"Success! You now have {wordsGameEngine.Points} {(wordsGameEngine.Points == 1 ? "point" : "points")}.");
                }
                else
                {
                    Console.WriteLine("Such word does not exist!");
                }
            }
            catch (Exception e)
            {
                if (e is WordHasBeenSubmittedException || e is InvalidLettersUsedException)
                {
                    Console.WriteLine(e.Message);
                    return;
                }

                if (e is UnknownApiResponseException)
                {
                    Console.WriteLine($"{e.Message}\nPlease try again.");
                }
            }
        }
        private static void HandleEnded(WordsGameEngine wordsGameEngine)
        {
            Console.WriteLine($"Game ended, you earned a total of {wordsGameEngine.Points} points.");
            Console.WriteLine("\nPress 'Enter' to start new game.");
            Console.ReadLine();
            wordsGameEngine.StartGame();
        }
    }
}
