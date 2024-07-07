using System;
using System.Collections.Generic;

namespace Theater.Application.Modules.PosterModule.Commands.PosterEditCommand
{
    public class PosterEditRequestDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int GenreId { get; set; }
        public string Duration { get; set; }
        public string Age { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<ShowDateEditDto> ShowDates { get; set; }

    }
}
