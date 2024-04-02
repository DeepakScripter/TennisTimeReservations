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

namespace TennisCourtBookingApp.Provider.Provider
{
     public class BookingProvider:IBookingProvider
    {
        protected UnitOfWork unitofwork = new UnitOfWork();
        private readonly IMapper _mapper;
        public BookingProvider(IMapper mapper)
        {
            _mapper = mapper;
        }
          public string SaveBooking(TennisCourtBookingModel model, int? userId , int? courtId)
          {
            if (model != null)
            {
                
                var CourtIdIsExist = unitofwork.TennisCourtBooking.GetAll(find=>find.BookingId==model.BookingId).FirstOrDefault();
                
                if (CourtIdIsExist == null)
                {



                    var bookingDetails = new TennisCourtBooking()
                    {
                        BookingDate = model.BookingDate,
                        UserId = userId,
                        TennisCourtId = courtId,
                        BookingTime = model.BookingTime,
                        Status ="Pending"
                    };
                unitofwork.TennisCourtBooking.Insert(bookingDetails);
                unitofwork.Save();
                    return null;
                }
                else
                {
                    var courtCapacity = unitofwork.TennisCourt.GetById(CourtIdIsExist.TennisCourtId);
                    var bookingDetails = unitofwork.TennisCourtBooking.GetAll(l => l.TennisCourtId == CourtIdIsExist.TennisCourtId).ToList();
                    var count = 0;
                    foreach (var booking in bookingDetails)
                    {
                        if (booking.BookingDate == model.BookingDate && booking.BookingTime == model.BookingTime && (booking.Status == "Confirm" || booking.Status == "Pending"))
                        {
                            count++;
                        }
                    }
                    if(count<courtCapacity.TennisCourtCapacity)
                    {
                        CourtIdIsExist.BookingDate = model.BookingDate;
                        CourtIdIsExist.BookingTime = model.BookingTime;


                        unitofwork.Save();
                        return "Success";
                    }
                    else
                    {
                        return "Slot Not Available";
                    }
                }

            }
            return null;
          }

        public List<TennisCourtBookingModel> GetBookCourtDetails(int? userId)
        {
            var fetchBookingDetails=unitofwork.TennisCourtBooking.GetAll(id=>id.UserId==userId).ToList();
            var bookingList = fetchBookingDetails.Select(bList => new TennisCourtBookingModel()
            {
                     UserId=bList.UserId,
                     BookingDate=bList.BookingDate,
                     BookingTime=bList.BookingTime,
                     TennisCourtId=bList.TennisCourtId,
                     BookingId=bList.BookingId,
                     Status=bList.Status
            }).ToList();
            return bookingList;
        }
        public TennisCourtBookingModel GetBookingById(int bookingId)
        {
            if(bookingId != null) {
            var BookingDetails=unitofwork.TennisCourtBooking.GetById(bookingId);
            var model = new TennisCourtBookingModel()
            {
                 BookingId=BookingDetails.BookingId,
                 UserId=BookingDetails.UserId,
                 BookingDate=BookingDetails.BookingDate,
                 BookingTime=BookingDetails.BookingTime,
                 TennisCourtId=BookingDetails.TennisCourtId
            };
            return model;
            }
            return null;
        }
        public void DeleteBooking(int bookingId)
        {
            var BookingDetails = unitofwork.TennisCourtBooking.GetById(bookingId);
            unitofwork.TennisCourtBooking.Delete(BookingDetails);
            unitofwork.Save();
            return;
        }


        public void DeleteBookingByCourtId(TennisCourtModel model)
        {
            var FindBookingId=unitofwork.TennisCourtBooking.GetAll(bId=>bId.TennisCourtId==model.TennisCourtId).ToList();

            if (FindBookingId.Count != null)
            { 
             unitofwork.TennisCourtBooking.DeleteAll(FindBookingId);
             unitofwork.Save();
            }
        }


        //public List<TennisCourtBookingModel> GetAllBookings(string Status)
        //{
        //    var BookingDetails = unitofwork.TennisCourtBooking.GetAll().ToList();
        //    var model = BookingDetails.Select(bd => new TennisCourtBookingModel()
        //    {
        //        UserId = bd.UserId,
        //        TennisCourtId = bd.TennisCourtId,
        //        BookingId = bd.BookingId,
        //        BookingDate = bd.BookingDate,
        //        BookingTime = bd.BookingTime,
        //        Status=bd.Status,
        //    }).ToList();

        //    if (Status == "Reject")
        //        model = model.Where(x => x.Status == "Reject").ToList();

        //    else if (Status == "Confirm")
            
        //        model = model.Where(x => x.Status == "Confirm").ToList();
           
        //   else if (Status == "Pending")
           
        //        model = model.Where(x => x.Status == "Pending").ToList();
            
        //    return model;
        //}


        public (int Count , int? Capacity) CheckSlotAvailability(TennisCourtBookingModel model, int courtId)
        {
            var courtCapacity = unitofwork.TennisCourt.GetById(courtId);
            var bookingDetails=unitofwork.TennisCourtBooking.GetAll(l=>l.TennisCourtId==courtId).ToList();            
            var count = 0;
            foreach(var booking in bookingDetails)
            {
                if (booking.BookingDate == model.BookingDate && booking.BookingTime == model.BookingTime&& (booking.Status=="Confirm" || booking.Status == "Pending"))
                {
                    count++;
                }
            }
            //if (count <= courtCapacity.TennisCourtCapacity)
            //{
            //    return c;
            //}
            //else
            //{
            //    return false;
            //}
            return (Count: count, Capacity: courtCapacity.TennisCourtCapacity);
        }


        //public DatatablePageResponseModel<TennisCourtBookingModel> GetUserBookings(DatatablePageRequestModel datatablePageRequest , int? userId)
        //{
        //    DatatablePageResponseModel<TennisCourtBookingModel> model = new DatatablePageResponseModel<TennisCourtBookingModel>()
        //    {
        //        draw = datatablePageRequest.Draw,
        //        data = new List<TennisCourtBookingModel>()
        //    };
        //    try
        //    {
        //        var userList = (from u in unitofwork.TennisCourtBooking.GetAll(id=>id.UserId==userId)
        //                        select new TennisCourtBookingModel()
        //                        {
        //                            TennisCourtId = u.TennisCourtId,
        //                          BookingId= u.BookingId,
        //                           BookingDate = u.BookingDate,
        //                            BookingTime = u.BookingTime,

        //                        });

        //        model.recordsTotal = userList.Count();
        //        if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
        //        {
        //            string searchTextWithoutSpaces = datatablePageRequest.SearchText.Replace(" ", "").ToLower();
        //            userList = userList.Where(x =>
        //                x.TennisCourtName.Replace(" ", "").ToLower().Contains(searchTextWithoutSpaces)
        //                || x.TennisCourtAddress.Replace(" ", "").ToLower().Contains(searchTextWithoutSpaces)
        //                 || x.UserName.Replace(" ", "").ToLower().Contains(searchTextWithoutSpaces)
        //                 || x.Status.Replace(" ", "").ToLower().Contains(searchTextWithoutSpaces)
        //                 || (x.BookingDate.HasValue && x.BookingDate.Value.ToString("MM/dd/yyyy").Contains(datatablePageRequest.SearchText))
        //                  || (x.BookingTime.HasValue && x.BookingTime.Value.ToString().Contains(datatablePageRequest.SearchText))
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

        public void EditBookStatus(String Status,int bookingId)
        {
            var bookingDetail = unitofwork.TennisCourtBooking.GetById(bookingId);

            if (bookingDetail != null)
            {
                bookingDetail.Status = Status;
                unitofwork.Save(); // Assuming you have a Save method in your unitofwork to persist changes
            }
        }
    }
}
