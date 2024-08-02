using MediatR;
using Theater.Application.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Theater.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Theater.Application.Modules.PosterModule.Commands.PosterRemoveCommand
{
    public class PosterRemoveRequestHandler : IRequestHandler<PosterRemoveRequest>
    {
        private readonly IPosterRepository _posterRepository;
        private readonly IShowDateRepository _showDateRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IRoleRepository _roleRepository; 

        public PosterRemoveRequestHandler(
            IPosterRepository posterRepository,
            IShowDateRepository showDateRepository,
            ITicketRepository ticketRepository,
            IRoleRepository roleRepository)
        {
            _posterRepository = posterRepository;
            _showDateRepository = showDateRepository;
            _ticketRepository = ticketRepository;
            _roleRepository = roleRepository; 
        }

        public async Task Handle(PosterRemoveRequest request, CancellationToken cancellationToken)
        {
            var poster = await _posterRepository.GetAll()
                .Include(p => p.ShowDates)
                .ThenInclude(sd => sd.Tickets)
                .Include(p => p.Roles) 
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.DeletedAt == null, cancellationToken);

            if (poster == null)
            {
                throw new NotFoundException("Poster entity not found.", request.Id);
            }

            foreach (var showDate in poster.ShowDates)
            {
                foreach (var ticket in showDate.Tickets)
                {
                    _ticketRepository.Remove(ticket);
                }
                _showDateRepository.Remove(showDate);
            }

            foreach (var role in poster.Roles)
            {
                _roleRepository.Remove(role);
            }

            _posterRepository.Remove(poster);

            await _ticketRepository.SaveAsync(cancellationToken);
            await _showDateRepository.SaveAsync(cancellationToken);
            await _roleRepository.SaveAsync(cancellationToken);
            await _posterRepository.SaveAsync(cancellationToken);
        }
    }
}
