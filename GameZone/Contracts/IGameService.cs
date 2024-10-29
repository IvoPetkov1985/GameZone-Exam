using GameZone.Data.DataModels;
using GameZone.Models;

namespace GameZone.Contracts
{
    public interface IGameService
    {
        Task AddGameToZoneAsync(Game gameToAddToZone, string userId);

        Task AddNewGameAsync(string userId, GameFormViewModel model);

        Task DeleteGameAsync(Game game);

        Task EditGameAsync(int id, EditGameViewModel model);

        Task<IEnumerable<AllGamesViewModel>> GetAllGamesAsync();

        Task<IEnumerable<GenreViewModel>> GetAllGenresAsync();

        Task<EditGameViewModel> GetEditViewModelAsync(int id);

        Task<Game> GetGameByIdAsync(int id);

        Task<DetailsGameViewModel> GetGameDetailsAsync(int id);

        Task<IEnumerable<MyZoneGameViewModel>> GetMyZoneGamesAsync(string userId);

        Task StrikeOutGameAsync(string userId, Game gameToStrikeOut);
    }
}
