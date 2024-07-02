using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using Theater.Domain.Models.Entities;

namespace Theater.Application.Modules.NewsModule.Commands.NewsEditCommand
{
    public class NewsEditRequest : IRequest<News>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishedAt { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
