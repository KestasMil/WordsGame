using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordsGameCore.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace WordsGameCore.Services.Tests
{
    [TestClass()]
    public class WordValidatorTests
    {
        private readonly WordValidator wordValidator;

        public WordValidatorTests()
        {
            // TODO: Mock HttpClient.
            HttpClient httpClient = new HttpClient();
            wordValidator = new WordValidator(httpClient);
        }

        [TestMethod()]
        [DataRow("test")]
        [DataRow("bEst")]
        [DataRow("TrusT")]
        [DataRow("Hello")]
        [DataRow("world")]
        public void IsValidWord_ValidWords_ReturnsTrue(string word)
        {
            var result = wordValidator.IsValidWord(word);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        [DataRow("ddd")]
        [DataRow("gfsadf")]
        [DataRow("Helic")]
        public void IsValidWord_InvalidWord_ReturnsFalse(string word)
        {
            var result = wordValidator.IsValidWord(word);
            Assert.IsFalse(result);
        }

        [TestMethod()]
        [DataRow(null)]
        public void IsValidWord_Null_ReturnsFalse(string word)
        {
            var result = wordValidator.IsValidWord(word);
            Assert.IsFalse(result);
        }

        [TestMethod()]
        [DataRow("")]
        public void IsValidWord_EmptyString_ReturnsFalse(string word)
        {

            var result = wordValidator.IsValidWord(word);
            Assert.IsFalse(result);
        }
    }
}