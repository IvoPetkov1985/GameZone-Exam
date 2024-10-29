using GameZone.Contracts;
using GameZone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static GameZone.Data.Common.DataConstants;

namespace GameZone.Controllers
{
    public class GameController : BaseController
    {
        private readonly IGameService service;

        public GameController(IGameService gameService)
        {
            service = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new GameFormViewModel();

            model.Genres = await service.GetAllGenresAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(GameFormViewModel model)
        {
            DateTime releasedOn;

            if (!DateTime.TryParseExact(model.ReleasedOn, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out releasedOn))
            {
                ModelState.AddModelError(nameof(model.ReleasedOn), "Invalid date format!");
            }

            if (!ModelState.IsValid)
            {
                model.Genres = await service.GetAllGenresAsync();

                return View(model);
            }

            string userId = GetUserId();

            await service.AddNewGameAsync(userId, model);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await service.GetAllGamesAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddToMyZone(int id)
        {
            var gameToAddToZone = await service.GetGameByIdAsync(id);

            if (gameToAddToZone == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            await service.AddGameToZoneAsync(gameToAddToZone, userId);

            return RedirectToAction(nameof(MyZone));
        }

        [HttpGet]
        public async Task<IActionResult> MyZone()
        {
            string userId = GetUserId();

            var model = await service.GetMyZoneGamesAsync(userId);

            if (!model.Any())
            {
                return RedirectToAction(nameof(All));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> StrikeOut(int id)
        {
            var gameToStrikeOut = await service.GetGameByIdAsync(id);

            if (gameToStrikeOut == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            await service.StrikeOutGameAsync(userId, gameToStrikeOut);

            return RedirectToAction(nameof(MyZone));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var gameToShow = await service.GetGameByIdAsync(id);

            if (gameToShow == null)
            {
                return BadRequest();
            }

            DetailsGameViewModel model = await service.GetGameDetailsAsync(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var gameToEdit = await service.GetGameByIdAsync(id);

            if (gameToEdit == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (userId != gameToEdit.PublisherId)
            {
                return Unauthorized();
            }

            EditGameViewModel modelToEdit = await service.GetEditViewModelAsync(id);

            return View(modelToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditGameViewModel model, int id)
        {
            var game = await service.GetGameByIdAsync(id);

            if (game == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (userId != game.PublisherId)
            {
                return Unauthorized();
            }

            await service.EditGameAsync(id, model);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var gameToDelete = await service.GetGameByIdAsync(id);

            if (gameToDelete == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (userId != gameToDelete.PublisherId)
            {
                return Unauthorized();
            }

            var model = new DeleteGameViewModel()
            {
                Id = gameToDelete.Id,
                Title = gameToDelete.Title
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DeleteGameViewModel model, int id)
        {
            var gameToDelete = await service.GetGameByIdAsync(id);

            if (gameToDelete == null)
            {
                return BadRequest();
            }

            await service.DeleteGameAsync(gameToDelete);

            return RedirectToAction(nameof(All));
        }
    }
}
