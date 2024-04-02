using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisCourtBookingApp.Common.BusinessEntities;
using TennisCourtBookingApp.Provider.IProvider;
using TennisCourtBookingApp.Repository.Models;
using TennisCourtBookingApp.Repository.Repository;

namespace TennisCourtBookingApp.Provider.Provider
{
    public class CommonProvider : ICommonProvider
    {

        protected UnitOfWork unitofwork = new UnitOfWork();
        private readonly IMapper _mapper;
        public CommonProvider(IMapper mapper)
        {
            _mapper = mapper;
        }



        public TennisCourtBookingRoleModel FindRole(TennisCourtBookingUserModel loginDetail)
        {
            var roleDetail = unitofwork.TennisCourtBookingUser.Get(u => u.Email == loginDetail.Email && u.Password == loginDetail.Password);


            if (roleDetail != null)
            {


                var roleName = unitofwork.TennisCourtBookingRole.GetById(roleDetail.RoleId);
                var Name = new TennisCourtBookingRoleModel()
                {
                    RoleName = roleName.RoleName,
                };
                return Name;
            }
            return null;
        }
        public int FindUser(TennisCourtBookingUserModel userDetails)
        {
            var details = unitofwork.TennisCourtBookingUser.GetAll(u => u.Email == userDetails.Email).FirstOrDefault();

            var userId = details.UserId;
            return userId;
        }
        public void RegisterUser(TennisCourtBookingUserModel signupDetails)
        {
            var newUser = new TennisCourtBookingUser()
            {
                UserName= signupDetails.UserName,
                Email= signupDetails.Email,
                Password= signupDetails.Password,
                Address= signupDetails.Address,
                PhoneNo= signupDetails.PhoneNo,
                RoleId=2,
                Image= signupDetails.Image,
                
            };
            unitofwork.TennisCourtBookingUser.Insert(newUser);
            unitofwork.Save();
            
        }

       
    }
}

