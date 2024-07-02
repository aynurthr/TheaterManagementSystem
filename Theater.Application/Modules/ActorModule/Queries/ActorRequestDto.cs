using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theater.Application.Modules.ActorModule.Queries
{
    public class ActorRequestDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string ImageSrc { get; set; }
        public IEnumerable<RoleRequestDto> Roles { get; set; }

    }

    public class RoleRequestDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public int PosterId { get; set; }
        public string PosterTitle { get; set; }
    }
}
