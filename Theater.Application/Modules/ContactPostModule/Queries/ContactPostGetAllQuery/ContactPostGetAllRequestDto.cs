using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theater.Application.Modules.ContactPostModule.Queries.ContactPostGetAllQuery
{
    public class ContactPostGetAllRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Topic { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? AnsweredAt { get; set; }
    }
}
