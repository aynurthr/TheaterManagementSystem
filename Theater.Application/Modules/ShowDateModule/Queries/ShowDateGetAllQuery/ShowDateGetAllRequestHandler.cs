using Theater.Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Modules.ShowDateModule.Queries;
using Theater.Application.Modules.TeamMemberModule.Queries.TeamMemberGetAllQuery;

namespace Theater.Application.Modules.ShowDateModule.Queries
{
    public class ShowDateGetAllRequestHandler : IRequestHandler<ShowDateGetAllRequest, IEnumerable<ShowDateDto>>
    {
        private readonly IShowDateRepository _showDateRepository;

        public ShowDateGetAllRequestHandler(IShowDateRepository showDateRepository)
        {
            _showDateRepository = showDateRepository;
        }

        public async Task<IEnumerable<ShowDateDto>> Handle(ShowDateGetAllRequest request, CancellationToken cancellationToken)
        {
            var query = _showDateRepository.GetAll();

            if (request.OnlyAvailable)
            {
                query = query.Where(m => m.DeletedAt == null);
            }

            var queryResponse = await query
                .Include(sd => sd.Poster)
                .Include(sd => sd.Hall)
                .Select(sd => new ShowDateDto
                {
                    Id = sd.Id,
                    PosterTitle = sd.Poster.Title,
                    Date = sd.Date,
                    HallName = sd.Hall.Name
                })
                .OrderBy(sd => sd.PosterTitle)
                .ToListAsync(cancellationToken);

            return queryResponse;
        }
    }
}
