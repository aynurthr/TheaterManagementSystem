namespace Theater.Application.Modules.PosterModule.Queries.PosterGetAllQuery
{
    public class PosterGetAllRequestDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ShowDate { get; set; }
        public string Age { get; set; }
        public string Genre { get; set; }
    }
}
