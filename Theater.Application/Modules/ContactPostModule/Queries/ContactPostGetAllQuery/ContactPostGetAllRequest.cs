using MediatR;

namespace Theater.Application.Modules.ContactPostModule.Queries.ContactPostGetAllQuery
{
    public class ContactPostGetAllRequest : IRequest<List<ContactPostGetAllRequestDto>>
    {
    }
}
