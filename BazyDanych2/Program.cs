using Microsoft.EntityFrameworkCore;

namespace BazyDanych2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using (var context = new GameContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var newPublisher = new Publisher
                {
                    Name = "Nintendo",
                };

                var newGame = new Game
                {
                    Title = "The Legend of Zelda",
                    Genre = "Adventure",
                    ReleaseYear = 1986,
                    Publisher = newPublisher,
                };
                // CREATE
                await context.Publishers.AddAsync(newPublisher);
                await context.Games.AddAsync(newGame);
                await context.SaveChangesAsync();
                Console.WriteLine("Dodano wydawcę i grę!");

                // READ
                var games = await context.Games
                    .Include(game => game.Publisher)
                    .Where(game => game.ReleaseYear > 1980)
                    .OrderBy(game => game.Title)
                    .ToListAsync();
                Console.WriteLine("Gry wydane po 1980, posortowane po tytule:");

                foreach (var game in games)
                {
                    Console.WriteLine($"Tytuł: {game.Title}, Wydawca: {game.Publisher.Name}");
                }

                // UPDATE
                var gameToUpdate = await context.Games.FirstAsync();
                gameToUpdate.Genre = "124336678";
                await context.SaveChangesAsync();

                Console.WriteLine($"Tytuł: {gameToUpdate.Title}, Gatunek: {gameToUpdate.Genre}");

                // DELETE
                var gameToDelete = await context.Games.FirstAsync();
                context.Games.Remove(gameToDelete);
                await context.SaveChangesAsync();
                Console.WriteLine("Usunięto grę");

            }
        }
    }
}
