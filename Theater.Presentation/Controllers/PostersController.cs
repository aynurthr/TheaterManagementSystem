using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Theater.Application.Modules.PosterModule.Queries.PosterGetAllQuery;
using Theater.Application.Modules.PosterModule.Queries.PosterGetByIdQuery;
using Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Theater.Application.Modules.CommentModule.Commands.CommentAddCommand;
using Theater.Domain.Models.Entities.Membership;
using Theater.Application.Modules.CommentModule.Commands.CommentEditCommand;
using Theater.Infrastructure.Abstracts;
using Theater.Repository;
using Theater.Application.Repositories;
using Theater.Application.Modules.GenreModule.Commands.GenreRemoveCommand;

namespace Theater.Presentation.Controllers
{
    public class PostersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PostersController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;


        public PostersController(IMediator mediator, ILogger<PostersController> logger, UserManager<AppUser> userManager, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _logger = logger;
            _userManager = userManager;
            _currentUserService = currentUserService;

        }

        [AllowAnonymous]

        public async Task<IActionResult> Index(PosterGetAllRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var request = new PosterGetByIdRequest { Id = id };
            var response = await _mediator.Send(request);

            if (response == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            ViewBag.CurrentUserName = user?.UserName ?? string.Empty;

            return View(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentAddRequest request)
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
        [HttpPost]
        public async Task<IActionResult> EditComment(CommentEditRequest request)
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
        [HttpPost]
        public async Task<IActionResult> RemoveComment(int id)
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

        public async Task<IActionResult> BuyTicket(int id)
        {
            _logger.LogInformation("BuyTicket called with PosterId: {PosterId}", id);
            var request = new PosterBuyTicketRequest { PosterId = id };
            var response = await _mediator.Send(request);

            if (response == null)
            {
                _logger.LogWarning("No response found for PosterId: {PosterId}", id);
                return NotFound();
            }

            _logger.LogInformation("Returning view for BuyTicket with PosterId: {PosterId}", id);
            return View(response);
        }

        [HttpGet("api/tickets/{showDateId}")]
        public async Task<IActionResult> GetTickets(int showDateId)
        {
            _logger.LogInformation("GetTickets called with ShowDateId: {ShowDateId}", showDateId);
            var request = new PosterBuyTicketRequest { ShowDateId = showDateId };
            var response = await _mediator.Send(request);

            if (response == null)
            {
                _logger.LogWarning("No tickets found for ShowDateId: {ShowDateId}", showDateId);
                return NotFound();
            }

            _logger.LogInformation("Returning JSON for GetTickets with ShowDateId: {ShowDateId}", showDateId);
            return Json(response);
        }
    }
}