using AutoMapper;
using TennisCourtBookingApp.Common.CommonEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisCourtBookingApp.Common.BusinessEntities;
using TennisCourtBookingApp.Provider.IProvider;
using TennisCourtBookingApp.Repository.Models;
using TennisCourtBookingApp.Repository.Repository;
using TennisCourtBookingApp.Common.Utility;

namespace TennisCourtBookingApp.Provider.Provider
{
    public class TennisCourtProvider : ITennisCourtProvider
    {

        protected UnitOfWork unitofwork = new UnitOfWork();
        private readonly IMapper _mapper;
        public TennisCourtProvider(IMapper mapper)
        {
            _mapper = mapper;
        }
        public void SaveTennisCourt(TennisCourtModel courtDetails)
        {
            var addCourt = new TennisCourt()
            {
                TennisCourtName = courtDetails.TennisCourtName,
                TennisCourtAddress = courtDetails.TennisCourtAddress,
                TennisCourtCapacity = courtDetails.TennisCourtCapacity
            };
            unitofwork.TennisCourt.Insert(addCourt);
            unitofwork.Save();
        }
        public List<TennisCourtModel> GetTennisCourtList()
        {
            var list = unitofwork.TennisCourt.GetAll().ToList();
            var modelList = list.Select(cl => new TennisCourtModel()
            {
                TennisCourtId = cl.TennisCourtId,
                TennisCourtName = cl.TennisCourtName,
                TennisCourtAddress = cl.TennisCourtAddress,
                TennisCourtCapacity = cl.TennisCourtCapacity

            }).ToList();
            return modelList;
        }
        public DatatablePageResponseModel<TennisCourtModel> GetList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<TennisCourtModel> model = new DatatablePageResponseModel<TennisCourtModel>()
            {
                draw = datatablePageRequest.Draw,
                data = new List<TennisCourtModel>()
            };
            try
            {
                var userList = (from u in unitofwork.TennisCourt.GetAll()
                                select new TennisCourtModel()
                                {
                                    TennisCourtId = u.TennisCourtId,
                                    TennisCourtName = u.TennisCourtName,
                                    TennisCourtAddress = u.TennisCourtAddress,
                                    TennisCourtCapacity = u.TennisCourtCapacity,

                                });

                model.recordsTotal = userList.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    int searchTextAsInt;
                    bool isNumeric = int.TryParse(datatablePageRequest.SearchText, out searchTextAsInt);
                    string searchTextWithoutSpaces = datatablePageRequest.SearchText.Replace(" ", "").ToLower();
                    userList = userList.Where(x =>
                     x.TennisCourtName.Replace(" ", "").ToLower().Contains(searchTextWithoutSpaces)
                     || x.TennisCourtAddress.Replace(" ", "").ToLower().Contains(searchTextWithoutSpaces)
                      || (isNumeric && x.TennisCourtCapacity.HasValue && x.TennisCourtCapacity.Value == searchTextAsInt)
                    );
                }

                model.recordsFiltered = userList.Count();

                model.data = userList.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    //x.Id = Protect(x.UserMasterId);
                    return x;
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }

        //public DatatablePageResponseModel<UserBookingDetailsModel> GetBookingList(DatatablePageRequestModel datatablePageRequest,string status)
        //{
        //    DatatablePageResponseModel<UserBookingDetailsModel> model = new DatatablePageResponseModel<UserBookingDetailsModel>()
        //    {
        //        draw = datatablePageRequest.Draw,
        //        data = new List<UserBookingDetailsModel>()
        //    };
        //    try
        //    {
        //        var userList = (from u in unitofwork.TennisCourtBooking.GetAll()
        //                        join user in unitofwork.TennisCourtBookingUser.GetAll() on u.UserId equals user.UserId
        //                        join court in unitofwork.TennisCourt.GetAll() on u.TennisCourtId equals court.TennisCourtId
        //                        select new UserBookingDetailsModel()
        //                        {
        //                            BookingId = u.BookingId,
        //                            BookingDate = u.BookingDate,
        //                            Status = u.Status,
        //                            BookingTime = u.BookingTime,
        //                            TennisCourtId = u.TennisCourtId,
        //                            UserId = u.UserId,

        //                            TennisCourtName = court.TennisCourtName,
        //                            TennisCourtAddress = court.TennisCourtAddress,

        //                            // Add other properties as needed
        //                        });

        //        model.recordsTotal = userList.Count();
        //        if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
        //        {
        //            userList = userList.Where(x =>
        //                x.TennisCourtName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
        //                || x.TennisCourtAddress.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
        //            // Add other conditions as needed
        //            );
        //        }

        //        model.recordsFiltered = userList.Count();

        //        model.data = userList.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return model;
        //}

        //public DatatablePageResponseModel<UserBookingDetailsModel> GetBookingList(DatatablePageRequestModel datatablePageRequest)
        //{
        //    DatatablePageResponseModel<UserBookingDetailsModel> model = new DatatablePageResponseModel<UserBookingDetailsModel>()
        //    {
        //        draw = datatablePageRequest.Draw,
        //        data = new List<UserBookingDetailsModel>()
        //    };
        //    try
        //    {
        //        var userList = (from u in unitofwork.TennisCourtBooking.GetAll()
        //                        select new UserBookingDetailsModel()
        //                        {
        //                            BookingId=u.BookingId,
        //                            BookingDate=u.BookingDate,
        //                            BookingTime=u.BookingTime,
        //                            TennisCourtId=u.TennisCourtId,
        //                            UserId=u.UserId,

        //                            //TennisCourtName = u.TennisCourtName,
        //                            //TennisCourtAddress = u.TennisCourtAddress,
        //                            //TennisCourtCapacity = u.TennisCourtCapacity,

        //                        });
        //        var userName = unitofwork.TennisCourtBookingUser.GetById(userList.UserId);

        //         //userList = (from u in unitofwork.TennisCourt.GetById(userList.TennisCourtId).FirstOrDefault()
        //         //               select new UserBookingDetailsModel()
        //         //               {
        //         //                   TennisCourtId = u.TennisCourtId,
        //         //                   TennisCourtName = u.TennisCourtName,
        //         //                   TennisCourtAddress = u.TennisCourtAddress,
        //         //                   //TennisCourtCapacity = u.TennisCourtCapacity,

        //         //               }).ToList() ;
        //         userList = (from u in unitofwork.TennisCourtBooking.GetAll()
        //                        select new UserBookingDetailsModel()
        //                        {
        //                            //TennisCourtId = u.TennisCourtId,
        //                            TennisCourtName = u.TennisCourtName,
        //                            TennisCourtAddress = u.TennisCourtAddress,
        //                            //TennisCourtCapacity = u.TennisCourtCapacity,

        //                        });

        //        model.recordsTotal = userList.Count();
        //        if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
        //        {
        //            userList = userList.Where(x =>
        //            x.TennisCourtName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
        //            || x.TennisCourtAddress.ToLower().Contains(datatablePageRequest.SearchText.ToLower())

        //            );
        //        }

        //        model.recordsFiltered = userList.Count();

        //        model.data = userList.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
        //        {
        //            //x.Id = Protect(x.UserMasterId);
        //            return x;
        //        }).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return model;
        //}

        public TennisCourtModel GetCourtById(int? courtID)
        {
            var courtDetails = unitofwork.TennisCourt.GetById(courtID);
            var courtModel = new TennisCourtModel()
            {
                TennisCourtId = courtDetails.TennisCourtId,
                TennisCourtName = courtDetails.TennisCourtName,
                TennisCourtAddress = courtDetails.TennisCourtAddress,
                TennisCourtCapacity = courtDetails.TennisCourtCapacity
            };
            return courtModel;
        }

        public void UpdateCourt(TennisCourtModel model)
        {
            if (model != null)
            {
                var courtDetails = unitofwork.TennisCourt.GetById(model.TennisCourtId);
                //var courtDetails =unitofwork.TennisCourt.GetAll(l=>l.TennisCourtId==model.TennisCourtId).FirstOrDefault();
                if (courtDetails != null)
                {
                    courtDetails.TennisCourtName = model.TennisCourtName;
                    courtDetails.TennisCourtAddress = model.TennisCourtAddress;
                    courtDetails.TennisCourtCapacity = model.TennisCourtCapacity;
                }
                unitofwork.Save();

            }
        }

        public void DeleteCourt(TennisCourtModel model)
        {
            if (model != null)
            {
                var courtDetails = unitofwork.TennisCourt.GetById(model.TennisCourtId);
                unitofwork.TennisCourt.Delete(courtDetails);
                unitofwork.Save();
            }
        }

        DatatablePageResponseModel<UserBookingDetailsModel> ITennisCourtProvider.GetBookingList(DatatablePageRequestModel datatablePageRequest, int status)
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
                                    Status = u.Status ?? "",
                                    BookingTime = u.BookingTime,
                                    TennisCourtId = u.TennisCourtId,
                                    UserId = u.UserId,
                                    UserName = user.UserName,

                                    TennisCourtName = court.TennisCourtName,
                                    TennisCourtAddress = court.TennisCourtAddress,

                                    // Add other properties as needed
                                });

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
                //if (status==3)
                //    userList = userList.Where(x => x.Status == "Reject");
                //if (status == 2)
                //    userList = userList.Where(x => x.Status == "Pending");
                //if (status == 4)
                //    userList = userList.Where(x => x.Status == "Confirm");
                model.recordsTotal = userList.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    string searchTextWithoutSpaces = datatablePageRequest.SearchText.Replace(" ", "").ToLower();
                    userList = userList.Where(x =>

                        x.TennisCourtName.Replace(" ", "").ToLower().Contains(searchTextWithoutSpaces)
                        || x.TennisCourtAddress.Replace(" ", "").ToLower().Contains(searchTextWithoutSpaces)
                         || x.UserName.Replace(" ", "").ToLower().Contains(searchTextWithoutSpaces)
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
