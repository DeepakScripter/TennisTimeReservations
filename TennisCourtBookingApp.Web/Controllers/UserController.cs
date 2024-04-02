using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TennisCourtBookingApp.Common.CommonEntities;
using System.Text.Json;
using TennisCourtBookingApp.Common.BusinessEntities;
using TennisCourtBookingApp.Provider.IProvider;
using TennisCourtBookingApp.Provider.Provider;
using TennisCourtBookingApp.Web.Models;


namespace TennisCourtBookingApp.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly ICommonProvider _commonProvider;
        private readonly IUserProvider _userProvider;
        private readonly ITennisCourtProvider _tennisCourtProvider;
        private readonly IBookingProvider _bookingProvider;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(ICommonProvider commonProvider , IUserProvider userProvider,ITennisCourtProvider tennisCourtProvider,IBookingProvider bookingProvider, IWebHostEnvironment webHostEnvironment) :base(commonProvider)
        {
            _commonProvider = commonProvider;
           _userProvider = userProvider;
            _tennisCourtProvider = tennisCourtProvider;
            _bookingProvider = bookingProvider;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult UserDetails(int? userId) {
            TennisCourtBookingViewModel viewModel=new TennisCourtBookingViewModel();
            if (HttpContext.Session.GetString("UserSession") == null)
            {
                return RedirectToAction("Login", "Home");
            }
             userId = HttpContext.Session.GetInt32("UserId");

            viewModel.TennisCourtBookingUserModel=_userProvider.GetUserById(userId);
            //viewModel.TennisCourtBookingModels = _bookingProvider.GetBookCourtDetails(userId);
            //viewModel.TennisCourtModels = _tennisCourtProvider.GetTennisCourtList();
            //TempData.Remove("List_items" + userId);

             return View(viewModel);
        }




        [HttpPost]
        public JsonResult GetUserBooking(int status)
        {
           int? userId = HttpContext.Session.GetInt32("UserId");
            var viewModel = _userProvider.GetUserBooking(GetPagingRequestModel(), userId,status);
            return Json(viewModel);
        }

        [HttpGet]
        public IActionResult UserProfile()
        {
            TennisCourtBookingViewModel viewModel = new TennisCourtBookingViewModel();
           int? userId = HttpContext.Session.GetInt32("UserId");
            viewModel.TennisCourtBookingUserModel = _userProvider.GetUserById(userId);

            return PartialView("_UserProfile",viewModel);
        }

        [HttpGet]
        public IActionResult UserEdit(int userId) {
        TennisCourtBookingViewModel model=new TennisCourtBookingViewModel();
            model.TennisCourtBookingUserModel= _userProvider.GetUserById(userId);
            model.IsEdit=true;
            model.IsUser = true;
            return PartialView("~/Views/Home/_UserPartial.cshtml",model);
            //return PartialView("_UserPartial", model);
        }

        [HttpPost]
        public IActionResult UserEdit(TennisCourtBookingViewModel model)
        {
            
           var userId = _userProvider.UpdateUser(model.TennisCourtBookingUserModel);
            //return RedirectToAction("UserDetails" , new {userId = userId});
            return Json(userId);
        }

       

            [HttpGet]
        public IActionResult ListedCourt()
        {

            TennisCourtBookingViewModel viewModel = new TennisCourtBookingViewModel();
          
          int?  userId = HttpContext.Session.GetInt32("UserId");

            viewModel.TennisCourtBookingUserModel = _userProvider.GetUserById(userId);
            viewModel.TennisCourtBookingModels = _bookingProvider.GetBookCourtDetails(userId);
            viewModel.TennisCourtModels = _tennisCourtProvider.GetTennisCourtList();
            viewModel.TempList = GetTempData(userId);
            //ViewBag.TempList = tempList;
            //TempData.Remove("SlotCheck");
            viewModel.tempDataKey = "SlotCheck" + userId;
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult GetList()
        {

            var model = GetPagingRequestModel();
            var viewModel = _tennisCourtProvider.GetList(model);
            return Json(viewModel);
        }
        [HttpGet]

        public IActionResult BookCourt(int courtId)
        {
            TennisCourtBookingViewModel viewModel = new TennisCourtBookingViewModel();
             viewModel.TennisCourtModel=_tennisCourtProvider.GetCourtById(courtId);
            return PartialView("_BookingCourtPartial",viewModel);
        }

        [HttpGet]
        public IActionResult EditBooking(int bookingId)
        {
            TennisCourtBookingViewModel viewModel = new TennisCourtBookingViewModel();
            viewModel.TennisCourtBookingModel = _bookingProvider.GetBookingById(bookingId);
           int? courtId = viewModel.TennisCourtBookingModel.TennisCourtId;
            viewModel.TennisCourtModel = _tennisCourtProvider.GetCourtById(courtId);
          viewModel.IsEdit= true;   

            return PartialView("_BookingCourtPartial", viewModel);
        }
        [HttpPost]
        public IActionResult EditBooking(TennisCourtBookingViewModel viewModel)
        {
           
           
            int? courtId = viewModel.TennisCourtBookingModel.TennisCourtId;
            int? userId = HttpContext.Session.GetInt32("UserId");
            var message=_bookingProvider.SaveBooking(viewModel.TennisCourtBookingModel, userId, courtId);

            return Json(message);
        }

        [HttpPost]
        public IActionResult DeleteCourtBooking(int bookingId)
        {
            _bookingProvider.DeleteBooking(bookingId);
            return RedirectToAction("UserDetails");
        }

        [HttpGet]

        public JsonResult AddBookingInTempData(TennisCourtBookingViewModel viewModel)
        {
            TennisCourtBookingViewModel model = new TennisCourtBookingViewModel();
            int? userId = HttpContext.Session.GetInt32("UserId");
            int courtId = viewModel.TennisCourtModel?.TennisCourtId ?? 0;
            var result = _bookingProvider.CheckSlotAvailability(viewModel.TennisCourtBookingModel,courtId);
            // Retrieve existing TempData
           

            List<TennisCourtBookingViewModel> tempList = GetTempData(userId);

            // Initialize the list if TempData is null
            if (tempList.Count ==0)
            {
                tempList = new List<TennisCourtBookingViewModel>();
            }

            // Set the user ID and court ID in the ViewModel
            viewModel.TennisCourtBookingModel.UserId = userId;
            viewModel.TennisCourtBookingModel.TennisCourtId = courtId;
            if (result.Count<result.Capacity)
            {
                foreach(var item in tempList)
                {
                    if (item != null) 
                    { 
                  var check=  item.TennisCourtBookingModel.TennisCourtId == viewModel.TennisCourtModel.TennisCourtId && item.TennisCourtBookingModel.BookingDate == viewModel.TennisCourtBookingModel.BookingDate && item.TennisCourtBookingModel.BookingTime == viewModel.TennisCourtBookingModel.BookingTime ;
                        if (check == true)
                        {
                            result.Count++;
                        }
                    }
                }
                if (result.Count < result.Capacity)
                { 
                    tempList.Add(viewModel); 
                }
                else
                {
                    model.tempDataKey = "SlotCheck" + userId;
                    TempData[model.tempDataKey] = "Slot Not Available!";
                    //model.Message = "false";
                }
                // Add the new booking to the list
               

            // Set the updated list in TempData
            SetTempData(tempList,userId);
                TempData["SlotCheck"] = "";
            }
            else
            {
                //model.Message = "false";
                model.tempDataKey = "SlotCheck" + userId;
                TempData[model.tempDataKey] = "Slot Not Available!";
            }
            return Json(model);
        }
        public IActionResult DeleteFromTempData(int id, DateTime bookingDate,TimeSpan bookingTime)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var itemlist = GetTempData(userId);
            TennisCourtBookingViewModel itemToRemove = itemlist.FirstOrDefault(item => item.TennisCourtBookingModel.TennisCourtId == id && item.TennisCourtBookingModel.BookingDate == bookingDate&& item.TennisCourtBookingModel.BookingTime == bookingTime);

            itemlist.Remove(itemToRemove);
            SetTempData(itemlist,userId);
            return Json("");
        }

        protected void SetTempData(List<TennisCourtBookingViewModel> bookings, int? id)
        {
            TempData["List_items"+id] = null;
            TempData["List_items"+id] = JsonSerializer.Serialize(bookings);
        }

        protected List<TennisCourtBookingViewModel> GetTempData(int? id)
        {
            if (TempData["List_items"+id] != null)
            {
                string data = TempData["List_items" + id].ToString();
                TempData.Keep();
                return JsonSerializer.Deserialize<List<TennisCourtBookingViewModel>>(data);
            }
            return new List<TennisCourtBookingViewModel>();
        }


        public IActionResult SubmitBookingsToDatabase()
        {
            TennisCourtBookingViewModel model=new TennisCourtBookingViewModel();
            int? userId = HttpContext.Session.GetInt32("UserId");
            // Retrieve all bookings from TempData
            List<TennisCourtBookingViewModel> bookings = GetTempData(userId);

            // Loop through the bookings and submit them to the database
            foreach (var booking in bookings)
            {
                //int? userId = HttpContext.Session.GetInt32("UserId");
                int? courtId = booking.TennisCourtModel?.TennisCourtId ?? 0;
                
                // The actual database save logic is separated into the _bookingProvider
                _bookingProvider.SaveBooking(booking.TennisCourtBookingModel, userId, courtId);
            }

            // Clear TempData after submitting to the database
            TempData.Remove("List_items"+userId);
            //TempData.Remove(model.tempDataKey);

            return Json(userId);
        }
        [HttpPost]
        public IActionResult EditBookingStatus(String status,int bookingId)
        {
            _bookingProvider.EditBookStatus(status,bookingId);
            return Json("");
        }
    }
}
