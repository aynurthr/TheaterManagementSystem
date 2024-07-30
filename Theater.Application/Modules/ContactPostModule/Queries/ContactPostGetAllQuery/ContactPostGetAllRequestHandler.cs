using MediatR;
using Microsoft.EntityFrameworkCore;
using Theater.Application.Repositories;

namespace Theater.Application.Modules.ContactPostModule.Queries.ContactPostGetAllQuery
{
    public class ContactPostGetAllRequestHandler : IRequestHandler<ContactPostGetAllRequest, List<ContactPostGetAllRequestDto>>
    {
        private readonly IContactPostRepository _contactPostRepository;

        public ContactPostGetAllRequestHandler(IContactPostRepository contactPostRepository)
        {
            _contactPostRepository = contactPostRepository;
        }

        public async Task<List<ContactPostGetAllRequestDto>> Handle(ContactPostGetAllRequest request, CancellationToken cancellationToken)
        {
            var contactPosts = await _contactPostRepository.GetAll()
                .OrderBy(cp => cp.AnsweredAt.HasValue)
                .ThenByDescending(cp => cp.SentAt)
                .ToListAsync(cancellationToken);

            return contactPosts.Select(cp => new ContactPostGetAllRequestDto
            {
                Id = cp.Id,
                Name = cp.Name,
                Email = cp.Email,
                Topic = cp.Topic,
                SentAt = cp.SentAt,
                AnsweredAt = cp.AnsweredAt,
            }).ToList();
        }
    }
}
