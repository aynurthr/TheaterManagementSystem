
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities.Membership;

namespace Theater.Application.Modules.ContactPostModule.Queries.ContactPostGetByIdQuery
{
    public class ContactPostGetByIdRequestHandler : IRequestHandler<ContactPostGetByIdRequest, ContactPostGetByIdRequestDto>
    {
        private readonly IContactPostRepository contactPostRepository;
        private readonly UserManager<AppUser> _userManager;

        public ContactPostGetByIdRequestHandler(IContactPostRepository contactPostRepository, UserManager<AppUser> userManager)
        {
            this.contactPostRepository = contactPostRepository;
            _userManager = userManager;
        }

        public async Task<ContactPostGetByIdRequestDto> Handle(ContactPostGetByIdRequest request, CancellationToken cancellationToken)
        {
            var entity = await contactPostRepository.GetAsync(m => m.Id == request.Id, cancellationToken);

            var AnsweredById = entity.AnsweredBy;
            var AnsweredByUserName = "Guest";

            var user = await _userManager.FindByIdAsync(AnsweredById);
            if (user != null)
            {
                AnsweredByUserName = user.UserName;
            }

            return new ContactPostGetByIdRequestDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Topic = entity.Topic,
                Message = entity.Message,
                SentAt = entity.SentAt,
                AnsweredAt = entity.AnsweredAt,
                AnsweredBy = AnsweredByUserName,
                Answer = entity.Answer,
            };
        }

    }
}