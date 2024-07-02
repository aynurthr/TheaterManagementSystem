using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theater.Application.Modules.TeamMemberModule.Queries
{
    public class TeamMemberRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Biography { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }

    }
}
