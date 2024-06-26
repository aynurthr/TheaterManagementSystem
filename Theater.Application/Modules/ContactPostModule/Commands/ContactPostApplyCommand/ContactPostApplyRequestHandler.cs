using MediatR;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;

namespace Theater.Application.Modules.ContactPostModule.Commands.ContactPostApplyCommand
{
    public class ContactPostApplyRequestHandler : IRequestHandler<ContactPostApplyRequest>
    {
        private readonly IContactPostRepository contactPostRepository;

        public ContactPostApplyRequestHandler(IContactPostRepository contactPostRepository)
        {
            this.contactPostRepository = contactPostRepository;
        }

        public async Task Handle(ContactPostApplyRequest request, CancellationToken cancellationToken)
        {
            var entity = new ContactPost();
            entity.Name = request.FullName;
            entity.Email = request.Email;
            entity.Topic = request.Subject;
            entity.Message = request.Message;
            entity.SentAt = DateTime.UtcNow;

            await contactPostRepository.AddAsync(entity, cancellationToken);
            await contactPostRepository.SaveAsync(cancellationToken);
        }
    }
}
