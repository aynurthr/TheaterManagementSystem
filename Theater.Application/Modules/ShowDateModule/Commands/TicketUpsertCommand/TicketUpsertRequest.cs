using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Theater.Application.Modules.ShowDateModule.Commands.UpsertTicketCommand
{
    public class UpsertTicketRequest : IRequest<bool>
    {
        [Required]
        public int ShowDateId { get; set; }

        [Required]
        public int SeatId { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
