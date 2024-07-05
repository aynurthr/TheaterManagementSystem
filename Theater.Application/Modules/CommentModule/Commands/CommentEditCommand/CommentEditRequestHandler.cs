using MediatR;
using Theater.Application.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Theater.Infrastructure.Exceptions;

namespace Theater.Application.Modules.CommentModule.Commands.CommentEditCommand
{
    public class CommentEditRequestHandler : IRequestHandler<CommentEditRequest>
    {
        private readonly ICommentRepository _commentRepository;

        public CommentEditRequestHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task Handle(CommentEditRequest request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetAsync(c => c.Id == request.Id && c.UserId == request.UserId, cancellationToken);

            if (comment == null)
            {
                throw new NotFoundException("Comment not found or you do not have permission to edit this comment.");
            }

            comment.CommentText = request.CommentText;
            _commentRepository.Edit(comment);
            await _commentRepository.SaveAsync(cancellationToken);

        }
    }
}
