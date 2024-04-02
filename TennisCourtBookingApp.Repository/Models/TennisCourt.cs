using System;
using System.Collections.Generic;

namespace TennisCourtBookingApp.Repository.Models
{
    public partial class TennisCourt
    {
        public TennisCourt()
        {
            TennisCourtBookings = new HashSet<TennisCourtBooking>();
        }

        public int TennisCourtId { get; set; }
        public string? TennisCourtName { get; set; }
        public string? TennisCourtAddress { get; set; }
        public int? TennisCourtCapacity { get; set; }

        public virtual ICollection<TennisCourtBooking> TennisCourtBookings { get; set; }
    }
}
