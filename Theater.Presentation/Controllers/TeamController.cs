using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Theater.Application.Modules.TeamMemberModule.Queries.TeamMemberGetAllQuery;

namespace Theater.Presentation.Controllers
{
    public class TeamController : Controller
    {
        private readonly IMediator _mediator;

        public TeamController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var request = new TeamMemberGetAllRequest();
            var response = await _mediator.Send(request);
            return View(response);
        }
    }
}

