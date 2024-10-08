﻿using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Concrates;

namespace Theater.Domain.Models.Entities
{
    public class Hall : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }

        public ICollection<Seat> Seats { get; set; }
        public ICollection<ShowDate> ShowDates { get; set; }
    }
}
