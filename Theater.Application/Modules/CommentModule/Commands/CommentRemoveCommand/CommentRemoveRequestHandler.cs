using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Modules.GenreModule.Commands.GenreRemoveCommand;
using Theater.Application.Repositories;
using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Exceptions;

namespace Theater.Application.Modules.CommentModule.Commands.CommentRemoveCommand
{
    public class CommentRemoveRequestHandler : IRequestHandler<CommentRemoveRequest>
    {
        private readonly ICommentRepository _commentRepository;

        public CommentRemoveRequestHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task Handle(CommentRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await _commentRepository.GetAsync(m => m.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException("Comment entity not found.", request.Id);
            }

            _commentRepository.Remove(entity);
            await _commentRepository.SaveAsync(cancellationToken);
        }
    }
}
