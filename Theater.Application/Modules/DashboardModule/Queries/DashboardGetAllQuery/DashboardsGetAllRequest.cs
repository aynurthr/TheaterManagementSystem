using MediatR;
using Theater.Domain.Models.DTOs;

namespace Theater.Application.Modules.DashboardModule.Queries.DashboardGetAllQuery
{
    public class DashboardGetAllRequest : IRequest<DashboardResponseDto>
    {
    }
}
