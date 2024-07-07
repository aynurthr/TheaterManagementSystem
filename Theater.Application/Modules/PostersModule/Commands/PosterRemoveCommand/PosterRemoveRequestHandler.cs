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

        public PosterRemoveRequestHandler(IPosterRepository posterRepository, IShowDateRepository showDateRepository, ITicketRepository ticketRepository)
        {
            _posterRepository = posterRepository;
            _showDateRepository = showDateRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task Handle(PosterRemoveRequest request, CancellationToken cancellationToken)
        {
            var poster = await _posterRepository.GetAll()
                .Include(p => p.ShowDates)
                .ThenInclude(sd => sd.Tickets)
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
            }

            foreach (var showDate in poster.ShowDates)
            {
                _showDateRepository.Remove(showDate);
            }

            _posterRepository.Remove(poster);

            await _ticketRepository.SaveAsync(cancellationToken);
            await _showDateRepository.SaveAsync(cancellationToken);
            await _posterRepository.SaveAsync(cancellationToken);
        }
    }
}
