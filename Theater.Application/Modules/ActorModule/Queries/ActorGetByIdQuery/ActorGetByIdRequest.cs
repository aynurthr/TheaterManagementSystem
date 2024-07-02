using MediatR;

namespace Theater.Application.Modules.ActorModule.Queries.ActorGetByIdQuery
{
    public class ActorGetByIdRequest : IRequest<ActorRequestDto>
    {
        public int Id { get; set; }
    }
}
