namespace Theater.Presentation.Models
{
    public class HomePageViewModel
    {
        public List<PosterViewModel> RecentPosters { get; set; }
        public List<NewsViewModel> RecentNews { get; set; }
        public bool MoreShowsAvailable { get; set; }

    }

    public class PosterViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ShowDate { get; set; }
        public string AgeRestriction { get; set; }
        public string Description { get; set; }
        public string Age { get; set; }

    }

    public class NewsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

}
