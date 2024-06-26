using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theater.Application.Modules.ContactPostModule.Commands.ContactPostApplyCommand
{
    public class ContactPostApplyRequest : IRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
