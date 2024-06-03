using System;
using System.Collections.Generic;

namespace TennisCourtBookingApp.Repository.Models
{
    public partial class ProjectDocument
    {
        public int DocumentId { get; set; }
        public int? ProjectId { get; set; }
        public string? DocumentName { get; set; }

        public virtual AssignTaskProject? Project { get; set; }
    }
}
