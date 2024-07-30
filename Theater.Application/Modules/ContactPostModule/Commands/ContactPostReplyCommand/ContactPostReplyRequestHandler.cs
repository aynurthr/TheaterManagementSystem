using MediatR;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Exceptions;

namespace Theater.Application.Modules.ContactPostModule.Commands.ContactPostReplyCommand
{
    public class ContactPostReplyRequestHandler : IRequestHandler<ContactPostReplyRequest>
    {
        private readonly IContactPostRepository _contactPostRepository;
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;

        public ContactPostReplyRequestHandler(
            IContactPostRepository contactPostRepository,
            IEmailService emailService,
            IIdentityService identityService)
        {
            _contactPostRepository = contactPostRepository;
            _emailService = emailService;
            _identityService = identityService;
        }

        public async Task Handle(ContactPostReplyRequest request, CancellationToken cancellationToken)
        {
            var contactPost = await _contactPostRepository.GetAsync(m => m.Id == request.Id, cancellationToken);
            if (contactPost == null)
            {
                throw new NotFoundException(nameof(ContactPost), request.Id);
            }

            contactPost.AnsweredAt = DateTime.Now;
            contactPost.AnsweredBy = _identityService.GetPrincipialId().ToString();
            contactPost.Answer = request.ReplyMessage;

            _contactPostRepository.Edit(contactPost);
            await _contactPostRepository.SaveAsync(cancellationToken);

            await _emailService.SendEmailAsync(
                contactPost.Email,
                $"Reply to your message: {contactPost.Topic}",
                request.ReplyMessage
            );

        }
    }
}
