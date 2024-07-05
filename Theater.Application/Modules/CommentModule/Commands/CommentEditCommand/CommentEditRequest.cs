using MediatR;

namespace Theater.Application.Modules.CommentModule.Commands.CommentEditCommand
{
    public class CommentEditRequest : IRequest
    {
        public int Id { get; set; }
        public int PosterId { get; set; } // PosterId for redirection
        public string CommentText { get; set; }
        public int UserId { get; set; } // UserId of the logged-in user
    }
}
