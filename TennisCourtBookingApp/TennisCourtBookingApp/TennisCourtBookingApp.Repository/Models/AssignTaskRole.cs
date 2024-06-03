using System;
using System.Collections.Generic;

namespace TennisCourtBookingApp.Repository.Models
{
    public partial class AssignTaskRole
    {
        public AssignTaskRole()
        {
            AssignTaskUsers = new HashSet<AssignTaskUser>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;

        public virtual ICollection<AssignTaskUser> AssignTaskUsers { get; set; }
    }
}
