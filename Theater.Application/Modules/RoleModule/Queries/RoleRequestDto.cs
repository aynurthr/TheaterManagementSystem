using System.Collections.Generic;

namespace Theater.Application.Modules.RoleModule.Queries
{
    public class RoleRequestDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public int ActorId { get; set; }
        public string ActorName { get; set; }
        public int PosterId { get; set; }
        public string PosterTitle { get; set; }
    }
}
