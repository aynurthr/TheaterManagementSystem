using MediatR;

namespace Theater.Application.Modules.GenreModule.Commands.GenreRemoveCommand
{
    public class GenreRemoveRequest : IRequest<string>
    {
        public int Id { get; set; }
    }
}
