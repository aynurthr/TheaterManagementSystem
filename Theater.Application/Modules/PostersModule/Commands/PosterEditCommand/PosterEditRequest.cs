using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Theater.Application.Modules.PosterModule.Commands.PosterEditCommand
{
    public class PosterEditRequest : IRequest<bool>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int GenreId { get; set; }
        public string Duration { get; set; }
        public string Age { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public List<ShowDateEditDto> ShowDates { get; set; }
    }

    public class ShowDateEditDto
    {
        public int ShowDateId { get; set; }
        public DateTime Date { get; set; }
        public int HallId { get; set; } // Even though HallId should not be changed, it's included for completeness.
    }
}
