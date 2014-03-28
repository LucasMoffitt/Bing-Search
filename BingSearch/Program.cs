using System;
using System.Collections.Generic;
using System.Configuration;

namespace BingSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiKey = ConfigurationManager.AppSettings["BingApi"];

            if (string.IsNullOrEmpty(apiKey))
                throw new NullReferenceException("Bing Api Key is missing!");

            var searcher = new Searcher(apiKey);

            do
            {
                Console.WriteLine("What are you looking for?");
                var query = Console.ReadLine();

                Console.WriteLine("How many results do you want? (leave empty for default)");
                int resultsCount;
                Int32.TryParse(Console.ReadLine(), out resultsCount);

                IEnumerable<BingSearch.Result> searchResults;
                try
                {
                    searchResults = searcher.Search(query, resultsCount);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    continue;
                }
                
                Console.WriteLine("Here's some results...");
                foreach (var result in searchResults)
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("Title: {0}", result.Title);
                    Console.WriteLine("Description: {0}", result.Description);
                    Console.WriteLine("Url: {0}", result.Url);
                }

            } while (true);
        }
    }
}
