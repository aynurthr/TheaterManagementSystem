using Microsoft.AspNetCore.Http;

namespace Theater.Application.Modules.PosterModule.Queries.PosterGetByIdQuery
{
    public class PosterGetByIdRequestDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }

        public int GenreId { get; set; }
        public string Duration { get; set; }
        public string Age { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public double Rating { get; set; }
        public IEnumerable<ActorDto> Actors { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
        public IEnumerable<ShowDateDto> ShowDates { get; set; }
    }

    public class ActorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Title { get; set; }
        public string ImageSrc { get; set; }
    }

    public class CommentDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ShowDateDto
    {
        public int HallId { get; set; }

        public int ShowDateId { get; set; }
        public DateTime Date { get; set; }
    }
}
