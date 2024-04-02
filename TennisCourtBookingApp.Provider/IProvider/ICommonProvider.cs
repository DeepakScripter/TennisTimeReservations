using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisCourtBookingApp.Common.BusinessEntities;

namespace TennisCourtBookingApp.Provider.IProvider
{
    public interface ICommonProvider
    {
        public TennisCourtBookingRoleModel FindRole(TennisCourtBookingUserModel loginDetail);
        public int FindUser(TennisCourtBookingUserModel userDetails);
        public void RegisterUser(TennisCourtBookingUserModel signupDetails);
        
    }
}
