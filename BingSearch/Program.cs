using System;
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
                var searchResults = searcher.Search(Console.ReadLine());

                if (searchResults == null)
                {
                    Console.WriteLine("Nothing to show...");
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
