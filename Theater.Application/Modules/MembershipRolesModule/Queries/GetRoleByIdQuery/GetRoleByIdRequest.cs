using MediatR;

namespace Theater.Application.Modules.RolesModule.Queries.GetRoleByIdQuery
{
    public class GetRoleByIdRequest : IRequest<GetRoleByIdResponse>
    {
        public int Id { get; set; }
        public string[] Policies { get; set; }
    }
}
