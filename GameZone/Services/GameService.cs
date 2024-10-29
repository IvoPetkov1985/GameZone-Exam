using GameZone.Contracts;
using GameZone.Data;
using GameZone.Data.DataModels;
using GameZone.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static GameZone.Data.Common.DataConstants;

namespace GameZone.Services
{
    public class GameService : IGameService
    {
        private readonly GameZoneDbContext context;

        public GameService(GameZoneDbContext dbContext)
        {
            context = dbContext;
        }

        public async Task AddGameToZoneAsync(Game gameToAddToZone, string userId)
        {
            if (!context.GamersGames.Any(gg => gg.GamerId == userId && gg.Game.Id == gameToAddToZone.Id))
            {
                var newEntry = new GamerGame()
                {
                    GameId = gameToAddToZone.Id,
                    GamerId = userId
                };

                await context.GamersGames.AddAsync(newEntry);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddNewGameAsync(string userId, GameFormViewModel model)
        {
            var gameToAdd = new Game()
            {
                Title = model.Title,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                ReleasedOn = DateTime.ParseExact(model.ReleasedOn, DateTimeFormat, CultureInfo.InvariantCulture),
                GenreId = model.GenreId,
                PublisherId = userId
            };

            await context.Games.AddAsync(gameToAdd);
            await context.SaveChangesAsync();
        }

        public async Task DeleteGameAsync(Game game)
        {
            context.Games.Remove(game);
            await context.SaveChangesAsync();
        }

        public async Task EditGameAsync(int id, EditGameViewModel model)
        {
            var game = await context.Games
                .Where(g => g.Id == id)
                .FirstAsync();

            game.Title = model.Title;
            game.Description = model.Description;
            game.ImageUrl = model.ImageUrl;
            game.ReleasedOn = DateTime.ParseExact(model.ReleasedOn, DateTimeFormat, CultureInfo.InvariantCulture);
            game.GenreId = model.GenreId;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllGamesViewModel>> GetAllGamesAsync()
        {
            var allGames = await context.Games
                .AsNoTracking()
                .Select(g => new AllGamesViewModel()
                {
                    Id = g.Id,
                    Title = g.Title,
                    ImageUrl = g.ImageUrl,
                    Genre = g.Genre.Name,
                    Publisher = g.Publisher.UserName,
                    ReleasedOn = g.ReleasedOn.ToString(DateTimeFormat)
                })
                .ToListAsync();

            return allGames;
        }

        public async Task<IEnumerable<GenreViewModel>> GetAllGenresAsync()
        {
            var genres = await context.Genres
                .AsNoTracking()
                .Select(g => new GenreViewModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                })
                .ToListAsync();

            return genres;
        }

        public async Task<EditGameViewModel> GetEditViewModelAsync(int id)
        {
            var genres = await context.Genres
                .Select(g => new GenreViewModel()
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();

            var gameModel = await context.Games
                .Where(g => g.Id == id)
                .Select(g => new EditGameViewModel()
                {
                    Title = g.Title,
                    Description = g.Description,
                    ImageUrl = g.ImageUrl,
                    ReleasedOn = g.ReleasedOn.ToString(DateTimeFormat),
                    GenreId = g.GenreId,
                    Genres = genres,
                    PublisherId = g.PublisherId
                })
                .FirstAsync();

            return gameModel;
        }

        public async Task<Game> GetGameByIdAsync(int id)
        {
            var game = await context.Games
                .AsNoTracking()
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            return game;
        }

        public async Task<DetailsGameViewModel> GetGameDetailsAsync(int id)
        {
            var gameToShow = await context.Games
                .AsNoTracking()
                .Where(g => g.Id == id)
                .Select(g => new DetailsGameViewModel()
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description,
                    ImageUrl = g.ImageUrl,
                    Genre = g.Genre.Name,
                    Publisher = g.Publisher.UserName,
                    ReleasedOn = g.ReleasedOn.ToString(DateTimeFormat)
                })
                .FirstAsync();

            return gameToShow;
        }

        public async Task<IEnumerable<MyZoneGameViewModel>> GetMyZoneGamesAsync(string userId)
        {
            var zonedGames = await context.GamersGames
                .AsNoTracking()
                .Where(gg => gg.GamerId == userId)
                .Select(gg => new MyZoneGameViewModel()
                {
                    Id = gg.Game.Id,
                    Title = gg.Game.Title,
                    ImageUrl = gg.Game.ImageUrl,
                    ReleasedOn = gg.Game.ReleasedOn.ToString(DateTimeFormat),
                    Genre = gg.Game.Genre.Name,
                    Publisher = gg.Game.Publisher.UserName
                })
                .ToListAsync();

            return zonedGames;
        }

        public async Task StrikeOutGameAsync(string userId, Game gameToStrikeOut)
        {
            if (context.GamersGames.Any(gg => gg.GamerId == userId && gg.Game.Id == gameToStrikeOut.Id))
            {
                GamerGame ggToRemove = await context.GamersGames
                    .Where(gg => gg.GamerId == userId && gg.GameId == gameToStrikeOut.Id)
                    .FirstAsync();

                context.GamersGames.Remove(ggToRemove);
                await context.SaveChangesAsync();
            }
        }
    }
}
