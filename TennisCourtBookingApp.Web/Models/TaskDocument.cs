using System;
using System.Collections.Generic;

namespace TennisCourtBookingApp.Web.Models
{
    public partial class TaskDocument
    {
        public int DocumentId { get; set; }
        public int? TaskId { get; set; }
        public string? DocumentName { get; set; }

        public virtual AssignTaskBug? Task { get; set; }
    }
}
