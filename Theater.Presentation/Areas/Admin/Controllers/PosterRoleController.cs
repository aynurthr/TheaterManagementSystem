using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using Theater.Application.Modules.RoleModule.Queries;
using Theater.Application.Modules.RoleModule.Commands.RoleAddCommand;
using Theater.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Theater.Application.Modules.RoleModule.Queries.RoleGetAllQuery;
using Theater.Application.Modules.RoleModule.Queries.RoleGetByIdQuery;
using Theater.Application.Modules.RoleModule.Commands.RoleEditCommand;
using Theater.Application.Modules.RoleModule.Commands.RoleRemoveCommand;
using Microsoft.AspNetCore.Authorization;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("poster-roles.manage")]

    public class PosterRoleController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IActorRepository _actorRepository;
        private readonly IPosterRepository _posterRepository;

        public PosterRoleController(IMediator mediator, IActorRepository actorRepository, IPosterRepository posterRepository)
        {
            _mediator = mediator;
            _actorRepository = actorRepository;
            _posterRepository = posterRepository;
        }

        // GET: Admin/PosterRole
        public async Task<IActionResult> Index()
        {
            var roles = await _mediator.Send(new RoleGetAllRequest());
            return View(roles);
        }

        // GET: Admin/PosterRole/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var role = await _mediator.Send(new RoleGetByIdRequest(id));
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // GET: Admin/PosterRole/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Actors = await _actorRepository.GetAll().ToListAsync();
            ViewBag.Posters = await _posterRepository.GetAll().ToListAsync();
            return View();
        }

        // POST: Admin/PosterRole/Create
        [HttpPost]
        public async Task<IActionResult> Create(RoleAddRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(request);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Actors = await _actorRepository.GetAll().ToListAsync();
            ViewBag.Posters = await _posterRepository.GetAll().ToListAsync();
            return View(request);
        }

        // GET: Admin/PosterRole/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _mediator.Send(new RoleGetByIdRequest(id));
            if (role == null)
            {
                return NotFound();
            }

            ViewBag.Actors = await _actorRepository.GetAll().ToListAsync();
            ViewBag.Posters = await _posterRepository.GetAll().ToListAsync();

            var model = new RoleEditRequest
            {
                Id = role.Id,
                RoleName = role.RoleName,
                ActorId = role.ActorId,
                PosterId = role.PosterId
            };

            return View(model);
        }

        // POST: Admin/PosterRole/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoleEditRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(request);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Actors = await _actorRepository.GetAll().ToListAsync();
            ViewBag.Posters = await _posterRepository.GetAll().ToListAsync();

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new RoleRemoveRequest(id));
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
