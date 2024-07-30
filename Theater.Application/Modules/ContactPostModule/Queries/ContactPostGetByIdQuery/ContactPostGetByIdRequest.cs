using MediatR;

namespace Theater.Application.Modules.ContactPostModule.Queries.ContactPostGetByIdQuery
{
    public class ContactPostGetByIdRequest : IRequest<ContactPostGetByIdRequestDto>
    {
        public int Id { get; set; }
    }
}
