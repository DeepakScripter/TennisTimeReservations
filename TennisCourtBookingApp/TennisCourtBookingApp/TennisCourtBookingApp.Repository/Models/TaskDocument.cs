using System;
using System.Collections.Generic;

namespace TennisCourtBookingApp.Repository.Models
{
    public partial class TaskDocument
    {
        public int DocumentId { get; set; }
        public int? TaskId { get; set; }
        public string? DocumentName { get; set; }

        public virtual AssignTaskBug? Task { get; set; }
    }
}
