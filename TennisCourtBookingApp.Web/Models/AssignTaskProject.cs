using System;
using System.Collections.Generic;

namespace TennisCourtBookingApp.Web.Models
{
    public partial class AssignTaskProject
    {
        public AssignTaskProject()
        {
            AssignTaskBugs = new HashSet<AssignTaskBug>();
            AssignTaskProjectAccesses = new HashSet<AssignTaskProjectAccess>();
            ProjectDocuments = new HashSet<ProjectDocument>();
        }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public int ProjectLeadId { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ProjectDeadLine { get; set; }
        public bool? IsActive { get; set; }

        public virtual AssignTaskUser ProjectLead { get; set; } = null!;
        public virtual ICollection<AssignTaskBug> AssignTaskBugs { get; set; }
        public virtual ICollection<AssignTaskProjectAccess> AssignTaskProjectAccesses { get; set; }
        public virtual ICollection<ProjectDocument> ProjectDocuments { get; set; }
    }
}
