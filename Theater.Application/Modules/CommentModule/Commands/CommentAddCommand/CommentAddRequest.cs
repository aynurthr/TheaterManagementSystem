using MediatR;

namespace Theater.Application.Modules.CommentModule.Commands.CommentAddCommand
{
    public class CommentAddRequest : IRequest
    {
        public int PosterId { get; set; }
        public string CommentText { get; set; }
        public int UserId { get; set; } // UserId of the logged-in user
    }
}
