using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Theater.Application.Modules.NewsModule.Queries.NewsGetAllQuery;
using Theater.Application.Modules.PosterModule.Queries.PosterGetAllQuery;
using System.Text.RegularExpressions;
using MediatR;
using Theater.Presentation.Models;

namespace Theater.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var postersRequest = new PosterGetAllRequest { OnlyAvailable = true };
            var postersResponse = await _mediator.Send(postersRequest);

            var newsRequest = new NewsGetAllRequest();
            var newsResponse = await _mediator.Send(newsRequest);

            var recentPosters = postersResponse.OrderBy(p => p.ShowDate)
                                                .Take(5)
                                                .Select(p => new PosterViewModel
                                                {
                                                    Id = p.Id,
                                                    Title = p.Title,
                                                    ImageUrl = p.ImageUrl,
                                                    ShowDate = p.ShowDate,
                                                    Description = TruncateDescription(StripHtmlTags(p.Description), 100),
                                                    Age = p.Age
                                                }).ToList();

            var recentNews = newsResponse.OrderByDescending(n => n.PublishedAt)
                                         .Take(4)
                                         .Select(n => new NewsViewModel
                                         {
                                             Id = n.Id,
                                             Title = n.Title,
                                             ImageUrl = n.ImageUrl,
                                             Date = n.PublishedAt,
                                             Description = TruncateDescription(StripHtmlTags(n.Description), 100)
                                         }).ToList();

            var viewModel = new HomePageViewModel
            {
                RecentPosters = recentPosters,
                RecentNews = recentNews,
                MoreShowsAvailable = postersResponse.Count() > 6 // Check if there are more than 6 posters

            };

            return View(viewModel);
        }

        private string StripHtmlTags(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }

        private string TruncateDescription(string description, int maxLength)
        {
            if (description.Length <= maxLength) return description;
            return description.Substring(0, maxLength) + "...";
        }

        public IActionResult RulesOfConduct()
        {
            return View();
        }

        public IActionResult HallPanorama()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

    }
}
