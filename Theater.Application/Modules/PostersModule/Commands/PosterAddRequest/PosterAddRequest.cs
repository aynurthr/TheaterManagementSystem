using MediatR;

namespace Theater.Application.Modules.PosterModule.Commands.PosterAddCommand
{
    public class PosterAddRequest : PosterAddRequestDto, IRequest<PosterAddRequestDto>
    {
    }
}
