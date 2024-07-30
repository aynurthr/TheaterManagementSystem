using MediatR;

namespace Theater.Application.Modules.ContactPostModule.Commands.ContactPostReplyCommand
{
    public class ContactPostReplyRequest : IRequest
    {
        public int Id { get; set; }
        public string ReplyMessage { get; set; }
    }
}
