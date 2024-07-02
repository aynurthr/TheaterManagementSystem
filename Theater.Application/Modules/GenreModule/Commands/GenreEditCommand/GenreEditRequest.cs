using MediatR;
using Theater.Application.Modules.GenreModule.Queries;

namespace Theater.Application.Modules.GenreModule.Commands.GenreEditCommand
{
    public class GenreEditRequest : IRequest<GenreRequestDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
