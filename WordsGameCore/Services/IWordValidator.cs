using System;
using System.Collections.Generic;
using System.Text;

namespace WordsGameCore.Services
{
    public interface IWordValidator
    {
        bool IsValidWord(string word);
    }
}