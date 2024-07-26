using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theater.Application.Modules.ShowDateModule.Queries
{
    public class SeatDto
    {
        public int Id { get; set; }
        public string Row { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
        public bool IsPurchased { get; set; }
        public bool IsTicketPurchased { get; set; }

    }
}
