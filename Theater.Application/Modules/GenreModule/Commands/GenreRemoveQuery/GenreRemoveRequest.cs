using MediatR;

namespace Theater.Application.Modules.GenreModule.Commands.GenreRemoveCommand
{
    public class GenreRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}
