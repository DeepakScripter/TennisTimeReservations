using System;
using System.Collections.Generic;

namespace TennisCourtBookingApp.Repository.Models
{
    public partial class AssignTaskUser
    {
        public AssignTaskUser()
        {
            AssignTaskBugAssignedToNavigations = new HashSet<AssignTaskBug>();
            AssignTaskBugCompletedByNavigations = new HashSet<AssignTaskBug>();
            AssignTaskProjectAccesses = new HashSet<AssignTaskProjectAccess>();
            AssignTaskProjects = new HashSet<AssignTaskProject>();
        }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public long PhoneNo { get; set; }
        public string? Password { get; set; }
        public int RoleId { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        public string? Image { get; set; }

        public virtual AssignTaskRole Role { get; set; } = null!;
        public virtual ICollection<AssignTaskBug> AssignTaskBugAssignedToNavigations { get; set; }
        public virtual ICollection<AssignTaskBug> AssignTaskBugCompletedByNavigations { get; set; }
        public virtual ICollection<AssignTaskProjectAccess> AssignTaskProjectAccesses { get; set; }
        public virtual ICollection<AssignTaskProject> AssignTaskProjects { get; set; }
    }
}
