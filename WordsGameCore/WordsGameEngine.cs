using System;
using System.Collections.Generic;
using System.Threading;
using WordsGameCore.Services;
using System.Linq;
using WordsGameCore.Exceptions;

namespace WordsGameCore
{
    public enum GameStatus
    {
        NotStarted,
        InProgress,
        Ended
    }
    public class WordsGameEngine
    {
        public int Points { get; private set; }
        public List<char> Letters { get; private set; }
        public GameStatus GameStatus { get; private set; }

        private List<string> wordsSubmitted;
        private readonly int timeLimitInSeconds;
        private readonly IWordValidator wordValidator;
        private readonly IScheduler scheduler;
        private readonly char[] consonants = { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'x', 'z', 'w', 'y' };
        private readonly char[] vowels = { 'a', 'e', 'i', 'o', 'u' };
        private readonly int numOfVowels;
        private readonly int numOfConsonants;
        /*
         * I refered to this article in order to come up with decent consonants to vowels ratio which should give fare chance for a player on each round.
         https://www.leozqin.me/vowel-compressibility-and-the-top-5000-words-in-english/
         */
        private const float vowelsPercentage = 31.45f;

        public WordsGameEngine(IWordValidator wordValidator, IScheduler scheduler, int timeLimitInSeconds = 90, int numberOfLetters = 12)
        {
            this.wordValidator = wordValidator;
            this.scheduler = scheduler;
            this.timeLimitInSeconds = timeLimitInSeconds;

            numOfVowels = (int)(Math.Round(((float)numberOfLetters / 100) * vowelsPercentage));
            numOfConsonants = numberOfLetters - numOfVowels;
        }

        public void StartGame()
        {
            Points = 0;
            wordsSubmitted = new List<string>();
            Letters = new List<char>();

            Random random = new Random();

            for (int i = 0; i < numOfConsonants; i++)
            {
                Letters.Add(consonants[random.Next(0, consonants.Length)]);
            }

            for (int i = 0; i < numOfVowels; i++)
            {
                Letters.Add(vowels[random.Next(0, vowels.Length)]);
            }

            Letters.Sort();

            scheduler.OnSchedule += (sender,args) => {
                this.GameStatus = GameStatus.Ended;
            };
            scheduler.ScheduleFor(DateTime.Now.AddSeconds(timeLimitInSeconds));

            this.GameStatus = GameStatus.InProgress;
        }

        public bool SubmitWord(string word)
        {
            if (GameStatus != GameStatus.InProgress)
                throw new GameIsNotInProgressException("Game is not in progress!");

            if (string.IsNullOrEmpty(word))
                throw new InvalidLettersUsedException("Word submitted cannot be an empty string.");

            word = word.ToLower();
            var validLettersUsed = word.ToCharArray().All((c) => Letters.FindAll(ltr=>ltr == c).Count >= word.ToList().FindAll(ltr => ltr == c).Count);

            if (!validLettersUsed)
                throw new InvalidLettersUsedException("Only given letters should be used.");

            if (wordsSubmitted.Contains(word))
                throw new WordHasBeenSubmittedException("You can only submit a word once.");

            wordsSubmitted.Add(word);

            if (wordValidator.IsValidWord(word))
            {
                Points++;
                return true;
            }

            return false;
        }

        public int GetTimeRemaining()
        {
            return scheduler.SecondsRemaining();
        }
    }
}