using MediatR;

namespace Theater.Application.Modules.GenreModule.Commands.GenreRemoveCommand
{
    public class CommentRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}
