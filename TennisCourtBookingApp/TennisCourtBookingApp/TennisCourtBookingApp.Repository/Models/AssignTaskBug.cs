using System;
using System.Collections.Generic;

namespace TennisCourtBookingApp.Repository.Models
{
    public partial class AssignTaskBug
    {
        public AssignTaskBug()
        {
            TaskDocuments = new HashSet<TaskDocument>();
        }

        public int TaskId { get; set; }
        public string? TaskType { get; set; }
        public string? TaskDescription { get; set; }
        public string? DocumentsRelated { get; set; }
        public int? AssignedTo { get; set; }
        public DateTime? TimeLimit { get; set; }
        public int? Progress { get; set; }
        public int? CompletedBy { get; set; }
        public DateTime? CompletedOn { get; set; }
        public bool? IsActive { get; set; }
        public int? Projectld { get; set; }

        public virtual AssignTaskUser? AssignedToNavigation { get; set; }
        public virtual AssignTaskUser? CompletedByNavigation { get; set; }
        public virtual AssignTaskProject? ProjectldNavigation { get; set; }
        public virtual ICollection<TaskDocument> TaskDocuments { get; set; }
    }
}
