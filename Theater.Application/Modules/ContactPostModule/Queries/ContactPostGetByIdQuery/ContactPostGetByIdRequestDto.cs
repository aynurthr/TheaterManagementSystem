


using Microsoft.AspNetCore.Http;

namespace Theater.Application.Modules.ContactPostModule.Queries.ContactPostGetByIdQuery
{
    public class ContactPostGetByIdRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? AnsweredAt { get; set; }
        public string? AnsweredBy { get; set; }
        public string? Answer { get; set; }
        public string ReplyMessage { get; set; }

    }
}

