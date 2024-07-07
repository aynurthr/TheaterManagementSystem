using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Theater.Application.Modules.PosterModule.Commands.PosterAddCommand
{
    public class PosterAddRequestDto
    {
        public string Title { get; set; }
        public int GenreId { get; set; }
        public string Duration { get; set; }
        public string Age { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public List<RoleDto> Roles { get; set; }
        public List<ShowDateDto> ShowDates { get; set; }
    }

    public class RoleDto
    {
        public string RoleName { get; set; }
        public int ActorId { get; set; }
    }

    public class ShowDateDto
    {
        public DateTime Date { get; set; }
        public int HallId { get; set; }
    }
}
