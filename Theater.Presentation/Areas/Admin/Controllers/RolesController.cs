using Theater.Application.Modules.RolesModule.Queries.GetRoleByIdQuery;
using Theater.Presentation.Pipeline;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly IMediator mediator;

        public RolesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize("admin.roles.details")]
        public async Task<IActionResult> Details([FromRoute] GetRoleByIdRequest request)
        {
            request.Policies = AppClaimsTransformation.policies;

            var response = await mediator.Send(request);

            return View(response);
        }
    }
}
