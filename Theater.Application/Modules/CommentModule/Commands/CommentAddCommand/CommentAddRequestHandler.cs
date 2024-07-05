using MediatR;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Theater.Application.Modules.CommentModule.Commands.CommentAddCommand
{
    public class CommentAddRequestHandler : IRequestHandler<CommentAddRequest>
    {
        private readonly ICommentRepository _commentRepository;

        public CommentAddRequestHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task Handle(CommentAddRequest request, CancellationToken cancellationToken)
        {
            var comment = new Comment
            {
                PosterId = request.PosterId,
                CommentText = request.CommentText,
                UserId = request.UserId,
                Time = DateTime.Now
            };

            await _commentRepository.AddAsync(comment, cancellationToken);
            await _commentRepository.SaveAsync(cancellationToken);

        }
    }
}
