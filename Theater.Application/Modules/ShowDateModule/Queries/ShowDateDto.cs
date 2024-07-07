using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theater.Application.Modules.ShowDateModule.Queries
{
    public class ShowDateDto
    {
        public int Id { get; set; }
        public string PosterTitle { get; set; }
        public DateTime Date { get; set; }
        public string HallName { get; set; }
    }
}
