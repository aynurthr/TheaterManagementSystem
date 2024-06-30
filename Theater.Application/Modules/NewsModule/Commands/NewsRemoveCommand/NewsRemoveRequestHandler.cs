using MediatR;
using Theater.Application.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Theater.Infrastructure.Exceptions;

namespace Theater.Application.Modules.NewsModule.Commands.NewsRemoveCommand
{
    public class NewsRemoveRequestHandler : IRequestHandler<NewsRemoveRequest>
    {
        private readonly INewsRepository _newsRepository;

        public NewsRemoveRequestHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task Handle(NewsRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await _newsRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException("News entity not found.", request.Id);
            }

            _newsRepository.Remove(entity);
            await _newsRepository.SaveAsync(cancellationToken);

        }
    }
}
