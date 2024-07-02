using MediatR;
using Theater.Application.Modules.GenreModule.Queries;

namespace Theater.Application.Modules.GenreModule.Commands.GenreAddCommand
{
    public class GenreAddRequest : IRequest<GenreRequestDto>
    {
        public string Name { get; set; }
    }
}
