using MediatR;

namespace Theater.Application.Modules.RoleModule.Queries.RoleGetByIdQuery
{
    public class RoleGetByIdRequest : IRequest<RoleRequestDto>
    {
        public int Id { get; set; }

        public RoleGetByIdRequest(int id)
        {
            Id = id;
        }
    }
}
