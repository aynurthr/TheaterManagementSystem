
using MediatR;
using System.Collections.Generic;
using Theater.Application.Modules.RoleModule.Queries;

namespace Theater.Application.Modules.RoleModule.Queries.RoleGetAllQuery
{
    public class RoleGetAllRequest : IRequest<List<RoleRequestDto>>
    {
    }
}
