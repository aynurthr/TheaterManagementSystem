using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Theater.Application.Modules.CommentModule.Commands.CommentAddCommand;
using Theater.Application.Modules.CommentModule.Commands.CommentEditCommand;
using Theater.Application.Modules.GenreModule.Commands.GenreRemoveCommand;
using Theater.Application.Modules.GenreModule.Queries.GenreGetAllQuery;
using Theater.Application.Modules.PosterModule.Commands.PurchaseTicketCommand;
using Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketQuery;
using Theater.Application.Modules.PosterModule.Queries.PosterGetAllQuery;
using Theater.Application.Modules.PosterModule.Queries.PosterGetByIdQuery;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities.Membership;

namespace Theater.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PostersController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IGenreRepository _genreRepository;

        public PostersController(IMediator mediator, ILogger<PostersController> logger, UserManager<AppUser> userManager, ICurrentUserService currentUserService, IGenreRepository genreRepository)
        {
            _mediator = mediator;
            _logger = logger;
            _userManager = userManager;
            _currentUserService = currentUserService;
            _genreRepository = genreRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery] PosterGetAllRequest request)
        {
            var response = await _mediator.Send(request);
            var genres = _genreRepository.GetAll().Where(g => g.DeletedAt == null);

            ViewBag.Genres = genres.Select(g => new { g.Id, g.Name }).ToList();
            ViewBag.SelectedGenre = genres.FirstOrDefault(g => g.Id == request.GenreId)?.Name ?? "All Genres";

            return View(response);
        }

        [HttpGet("genre/{genreId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostersByGenre(int genreId)
        {
            if (genreId == 0)
            {
                return RedirectToAction("Index");
            }

            var request = new PosterGetAllRequest { GenreId = genreId };
            var response = await _mediator.Send(request);
            var genres = _genreRepository.GetAll().Where(g => g.DeletedAt == null);

            ViewBag.Genres = genres.Select(g => new { g.Id, g.Name }).ToList();
            ViewBag.SelectedGenre = genres.FirstOrDefault(g => g.Id == genreId)?.Name ?? "All Genres";

            return View("Index", response);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchPosters([FromQuery] string query)
        {
            var request = new PosterGetAllRequest { SearchQuery = query };
            var response = await _mediator.Send(request);
            var genres = _genreRepository.GetAll().Where(g => g.DeletedAt == null);

            ViewBag.Genres = genres.Select(g => new { g.Id, g.Name }).ToList();
            ViewBag.SelectedGenre = "Search Results";

            return View("Index", response);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var request = new PosterGetByIdRequest { Id = id };
            var response = await _mediator.Send(request);

            if (response == null)
            {
                return View("NotFound");
            }

            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            ViewBag.CurrentUserName = user?.UserName ?? string.Empty;

            return View(response);
        }

        [Authorize]
        [HttpPost("add-comment")]
        public async Task<IActionResult> AddComment([FromForm] CommentAddRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("You must be logged in to add a comment.");
            }

            request.UserId = user.Id;
            await _mediator.Send(request);

            return RedirectToAction("Details", new { id = request.PosterId });
        }



        [Authorize]
        [HttpPost("edit-comment")]
        public async Task<IActionResult> EditComment([FromForm] CommentEditRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("You must be logged in to edit a comment.");
            }

            request.UserId = user.Id;
            await _mediator.Send(request);

            return RedirectToAction("Details", new { id = request.PosterId });
        }


        [Authorize]
        [HttpPost("remove-comment")]
        public async Task<IActionResult> RemoveComment([FromForm] int id)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                return Unauthorized();
            }

            var request = new CommentRemoveRequest
            {
                Id = id
            };

            await _mediator.Send(request);

            return Ok();
        }



        [HttpGet("buy-ticket/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> BuyTicket(int id)
        {
            var posterDetailsRequest = new PosterGetByIdRequest { Id = id };
            var posterDetails = await _mediator.Send(posterDetailsRequest);

            if (posterDetails == null || !posterDetails.ShowDates.Any())
            {
                return View("NotFound");
            }
            var showDateId = posterDetails.ShowDates.First().ShowDateId;

            var request = new PosterBuyTicketRequest { PosterId = id, ShowDateId = showDateId };

            var response = await _mediator.Send(request);

            if (response == null)
            {
                return View("NotFound");
            }
            return View(response);
        }


        [HttpGet("{posterId}/tickets/{showDateId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTickets(int posterId, int showDateId)
        {
            var request = new PosterBuyTicketRequest
            {
                PosterId = posterId,
                ShowDateId = showDateId
            };

            var response = await _mediator.Send(request);

            if (response == null)
            {
                return NotFound();
            }

            return Json(response);
        }

        [HttpPost("purchase-tickets")]
        [Authorize]
        public async Task<IActionResult> PurchaseTickets([FromBody] PurchaseTicketRequest command)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            command.UserId = user.Id;
            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest("Unable to purchase tickets. Please try again.");
            }

            return Ok("Tickets purchased successfully.");
        }
    }
}
