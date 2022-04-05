using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordsGameCore;
using System;
using System.Collections.Generic;
using System.Text;
using WordsGameCore.Services;
using Moq;
using System.Linq;
using WordsGameCore.Exceptions;

namespace WordsGameCore.Tests
{
    [TestClass()]
    public class WordsGameEngineTests
    {
        private WordsGameEngine engine;
        private readonly Mock<IWordValidator> wordValidatorMock = new Mock<IWordValidator>();
        private readonly Mock<IScheduler> schedulerMock = new Mock<IScheduler>();
        private readonly List<char> alphabet = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'y', 'z' };

        [TestInitialize]
        public void InitGameEngine()
        {
            engine = new WordsGameEngine(wordValidatorMock.Object, schedulerMock.Object, 10);
        }

        [TestMethod()]
        public void Ctor_GameInitialized_CorrectGameStatus()
        {
            Assert.AreEqual(GameStatus.NotStarted, engine.GameStatus);
        }

        [TestMethod()]
        public void StartGame_GameStarted_CorrectGameStatus()
        {
            engine.StartGame();
            Assert.AreEqual(GameStatus.InProgress, engine.GameStatus);
        }

        [TestMethod()]
        public void StartGame_GameEnds_CorrectGameStatus()
        {
            engine.StartGame();
            schedulerMock.Raise(sm => sm.OnSchedule += null, EventArgs.Empty);
            Assert.AreEqual(GameStatus.Ended, engine.GameStatus);
        }

        [TestMethod()]
        public void GetTimeRemaining_GameInProgress_CorrectRemainingTime()
        {
            engine.StartGame();
            schedulerMock.Setup(sm => sm.SecondsRemaining()).Returns(4);
            Assert.AreEqual(4, engine.GetTimeRemaining());
        }

        [TestMethod()]
        public void SubmitWord_NotAvailableLettersUsed_ThrowsInvalidLettersUsedException()
        {
            engine.StartGame();
            var randomInvalidLettersWord = string.Join("", alphabet.Where(c => !engine.Letters.Contains(c)).Take(5));
            Assert.ThrowsException<InvalidLettersUsedException>(() => engine.SubmitWord(randomInvalidLettersWord));
        }

        [TestMethod()]
        public void SubmitWord_SubmittedWordTwice_ThrowsWordHasBeenSubmittedException()
        {
            engine.StartGame();
            var validLetter = engine.Letters.First().ToString();
            engine.SubmitWord(validLetter);
            Assert.ThrowsException<WordHasBeenSubmittedException>(() => engine.SubmitWord(validLetter));
        }
    }
}