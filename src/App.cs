using Microsoft.Extensions.Hosting;
using MovieRating.Domain;
using MovieRating.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Template
{
    public class App : IHostedService
    {
        private readonly IOmdbService<Movie> _omdbService;

        public App(IOmdbService<Movie> omdbService)
        {
            _omdbService = omdbService;
        }

        public async Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            do
            {
                Console.WriteLine("Enter Movie Title");
                var title = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(title))
                {
                    var response = await _omdbService.GetRating(title);

                    if (!string.IsNullOrWhiteSpace(response.Title))
                    {
                        Console.WriteLine($"OMDB Movie Rating is {response.imdbRating} released on {response.Year}");
                        Console.WriteLine("Do you want to continue again? (Y/N)");
                    }
                    else
                    {
                        Console.WriteLine($"No Movie found!");
                        Console.ReadKey();
                    }
                }
            }
            while (Console.ReadKey().Key == ConsoleKey.Y);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
