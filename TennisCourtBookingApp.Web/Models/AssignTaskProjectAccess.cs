using System;
using System.Collections.Generic;

namespace TennisCourtBookingApp.Web.Models
{
    public partial class AssignTaskProjectAccess
    {
        public int ProjectId { get; set; }
        public int AccessTold { get; set; }
        public int ProjectAccessId { get; set; }

        public virtual AssignTaskUser AccessToldNavigation { get; set; } = null!;
        public virtual AssignTaskProject Project { get; set; } = null!;
    }
}
