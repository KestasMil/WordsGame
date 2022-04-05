using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WordsGameCore.Services;

namespace WordsGameCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void UseWordsGameEngineDefaults(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<HttpClient>()
                    .AddSingleton<IWordValidator, WordValidator>()
                    .AddSingleton<IScheduler, Scheduler>()
                    .AddSingleton<WordsGameEngine>();
        }
    }
}
