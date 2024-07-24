using MediatR;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.DashboardModule.Queries.DashboardGetAllQuery;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardData = await _mediator.Send(new DashboardGetAllRequest());
            return View(dashboardData);
        }
    }
}
