using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisCourtBookingApp.Common.BusinessEntities;
using TennisCourtBookingApp.Common.CommonEntities;
using TennisCourtBookingApp.Common.Utility;
using TennisCourtBookingApp.Provider.IProvider;
using TennisCourtBookingApp.Repository.Models;
using TennisCourtBookingApp.Repository.Repository;
using TennisCourtBookingApp.Common.Utility;

namespace TennisCourtBookingApp.Provider.Provider
{

  
    public class UserProvider : IUserProvider
    {
         protected UnitOfWork unitofwork = new UnitOfWork();
        private readonly IMapper _mapper;
        public UserProvider(IMapper mapper)
        {
            _mapper = mapper;
        }
        public TennisCourtBookingUserModel GetUserById(int? userId)
        {
            var userDetails=unitofwork.TennisCourtBookingUser.GetById(userId);
            var model = new TennisCourtBookingUserModel()
            {
                UserId=userDetails.UserId,
                UserName=userDetails.UserName,
                Email=userDetails.Email,
                Address=userDetails.Address,
                PhoneNo=userDetails.PhoneNo,
                Password=userDetails.Password,
                Image=userDetails.Image,
            };
            return model;
        }


        public int UpdateUser(TennisCourtBookingUserModel model)
        {
            var userDetails = unitofwork.TennisCourtBookingUser.GetById(model.UserId);
            if(userDetails!= null) {
              userDetails.UserName = model.UserName;
                userDetails.Email = model.Email;
                userDetails.Address = model.Address;
                userDetails.PhoneNo = model.PhoneNo;
                userDetails.Password = model.Password;
            }
            unitofwork.Save();
            return model.UserId;
        }


        public List<TennisCourtBookingUserModel> GetUsersList()
        {
            var userList=unitofwork.TennisCourtBookingUser.GetAll().ToList();
            var model = userList.Select(user => new TennisCourtBookingUserModel()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Address = user.Address,
                PhoneNo = user.PhoneNo,
                Password = user.Password

            }).ToList();
            return model;
        }
        public void UpdateUserImage(TennisCourtBookingUserModel model, int? userId)
        {
            var userDetails = unitofwork.TennisCourtBookingUser.GetById(userId);
            if(userDetails!= null)
            {
                userDetails.Image= model.Image;
            }
            unitofwork.Save();
        }


        DatatablePageResponseModel<UserBookingDetailsModel> IUserProvider.GetUserBooking(DatatablePageRequestModel datatablePageRequest, int? userId, int status)
        {
            DatatablePageResponseModel<UserBookingDetailsModel> model = new DatatablePageResponseModel<UserBookingDetailsModel>()
            {
                draw = datatablePageRequest.Draw,
                data = new List<UserBookingDetailsModel>()
            };
            try
            {
                var userList = (from u in unitofwork.TennisCourtBooking.GetAll()
                                join user in unitofwork.TennisCourtBookingUser.GetAll() on u.UserId equals user.UserId
                                join court in unitofwork.TennisCourt.GetAll() on u.TennisCourtId equals court.TennisCourtId
                                select new UserBookingDetailsModel()
                                {
                                    BookingId = u.BookingId,
                                    BookingDate = u.BookingDate,
                                    Status = u.Status??"",
                                    BookingTime = u.BookingTime,
                                    TennisCourtId = u.TennisCourtId,
                                    UserId = u.UserId,
                                    UserName = user.UserName,

                                    TennisCourtName = court.TennisCourtName,
                                    TennisCourtAddress = court.TennisCourtAddress,

                                    // Add other properties as needed
                                });
                userList = userList.Where(x => x.UserId == userId);
                switch ((BookingStatus)status)
                {
                    case BookingStatus.Pending:
                        userList = userList.Where(x => x.Status == BookingStatus.Pending.ToString());
                        break;
                    case BookingStatus.Confirm:
                        userList = userList.Where(x => x.Status == BookingStatus.Confirm.ToString());
                        break;
                    case BookingStatus.Rejected:
                        userList = userList.Where(x => x.Status == BookingStatus.Rejected.ToString());
                        break;
                    case BookingStatus.All:
                        // Handle the "All" case if needed
                        break;
                    default:
                        // Handle other cases
                        break;
                }
                //if (status == "Reject")
                //    userList = userList.Where(x => x.Status == "Reject");
                //if (status == "Pending")
                //    userList = userList.Where(x => x.Status == "Pending");
                //if (status == "Confirm")
                //    userList = userList.Where(x => x.Status == "Confirm");
                model.recordsTotal = userList.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    string searchTextWithoutSpaces = datatablePageRequest.SearchText.Replace(" ", "").ToLower();
                    userList = userList.Where(x =>
                        x.TennisCourtName.Replace(" ", "").ToLower().Contains(searchTextWithoutSpaces)
                        || x.TennisCourtAddress.Replace(" ", "").ToLower().Contains(searchTextWithoutSpaces)
                         || x.Status.Replace(" ", "").ToLower().Contains(searchTextWithoutSpaces)
                         //|| (x.BookingDate.HasValue && x.BookingDate.Value.ToString("MM/dd/yyyy").Contains(datatablePageRequest.SearchText))
                         // || (x.BookingTime.HasValue && x.BookingTime.Value.ToString().Contains(datatablePageRequest.SearchText))
                    // Add other conditions as needed
                    );
                }

                model.recordsFiltered = userList.Count();

                model.data = userList.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }
    }

}
