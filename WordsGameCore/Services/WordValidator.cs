using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WordsGameCore.Exceptions.WordValidator;

namespace WordsGameCore.Services
{
    public class WordValidator : IWordValidator
    {
        private HttpClient Client { get; set; }
        public WordValidator(HttpClient httpClient)
        {
            Client = httpClient;
        }
        public bool IsValidWord(string word)
        {
            // dictionaryapi.dev returns 200 http response to most (if not all) single character requests, this is a hack to prevent that behaviour.
            // TODO: replace dictionaryapi.dev with a different words validation service.
            if (word == null || word.Length <= 1)
                return false;

            var result = Task.Run(() => Client.GetAsync($"https://api.dictionaryapi.dev/api/v2/entries/en/{word}"));
            try
            {
                result.Wait();
            }
            catch (Exception e)
            {
                throw new UnknownApiResponseException("Could not reach api.dictionaryapi.dev", e);
            }
            
            switch (result.Result.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return true;
                case System.Net.HttpStatusCode.NotFound:
                    return false;
                default:
                    throw new UnknownApiResponseException("Unknown response from api.dictionaryapi.dev");
            }
        }
    }
}