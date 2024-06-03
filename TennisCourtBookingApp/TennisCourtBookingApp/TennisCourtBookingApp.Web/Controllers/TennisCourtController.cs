using Microsoft.AspNetCore.Mvc;
using TennisCourtBookingApp.Common.BusinessEntities;
using TennisCourtBookingApp.Provider.IProvider;
using TennisCourtBookingApp.Web.Models;
using TennisCourtBookingApp.Common.Utility;


namespace TennisCourtBookingApp.Web.Controllers
{
    public class TennisCourtController : BaseController
    {
        private readonly ICommonProvider _commonProvider;
        private readonly ITennisCourtProvider _tennisCourtProvider;
        private readonly IBookingProvider _bookingProvider;
        private readonly IUserProvider _userProvider;

        public TennisCourtController(ICommonProvider commonProvider, ITennisCourtProvider tennisCourtProvider, IBookingProvider bookingProvider, IUserProvider userProvider) : base(commonProvider)
        {
            _commonProvider = commonProvider;
            _tennisCourtProvider = tennisCourtProvider;
            _bookingProvider = bookingProvider;
            _userProvider = userProvider;
        }


        public IActionResult Index()
        {
            TennisCourtBookingViewModel viewModel = new TennisCourtBookingViewModel();
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //viewModel.TennisCourtModels= _tennisCourtProvider.GetTennisCourtList();
            //viewModel.TennisCourtBookingModels = _bookingProvider.GetAllBookings();
            //viewModel.TennisCourtModels = _tennisCourtProvider.GetTennisCourtList();
            //viewModel.TennisCourtBookingUserModels = _userProvider.GetUsersList();
            return View(viewModel);
        }
        [HttpPost]
        public JsonResult GetList(string searchText, int length)
        {

            var model = GetPagingRequestModel();
            var viewModel = _tennisCourtProvider.GetList(model,searchText,length);
            return Json(viewModel);
        }

        [HttpGet]
        public IActionResult AddCourt()
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            TennisCourtBookingViewModel viewModel = new TennisCourtBookingViewModel();
            viewModel.IsAdd = true;
            viewModel.IsView = false;
            return PartialView("_AddCourt", viewModel);
        }
        [HttpPost]
        public IActionResult AddCourt(TennisCourtBookingViewModel courtDetails)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            _tennisCourtProvider.SaveTennisCourt(courtDetails.TennisCourtModel,userId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult TennisCourtEdit(int courtId)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            TennisCourtBookingViewModel viewModel = new TennisCourtBookingViewModel();
            viewModel.TennisCourtModel = _tennisCourtProvider.GetCourtById(courtId);
            viewModel.IsEdit = true;
            viewModel.IsView = true;
            return PartialView("_AddCourt", viewModel);
        }

        [HttpPost]
        public IActionResult TennisCourtEdit(TennisCourtBookingViewModel model)
        {

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            _tennisCourtProvider.UpdateCourt(model.TennisCourtModel,userId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult TennisCourtDelete(int courtId)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            TennisCourtBookingViewModel viewModel = new TennisCourtBookingViewModel();
            viewModel.TennisCourtModel = _tennisCourtProvider.GetCourtById(courtId);
            viewModel.IsDelete = true;
            viewModel.IsView = true;
            return PartialView("_AddCourt", viewModel);
        }

        [HttpPost]
        public IActionResult TennisCourtDelete(TennisCourtBookingViewModel model)
        {
            TennisCourtBookingViewModel viewModel = new TennisCourtBookingViewModel();
            _bookingProvider.DeleteBookingByCourtId(model.TennisCourtModel);
            _tennisCourtProvider.DeleteCourt(model.TennisCourtModel);


            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult BookingDetails()
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //TennisCourtBookingViewModel viewModel = new TennisCourtBookingViewModel();
            //viewModel.TennisCourtBookingModels = _bookingProvider.GetAllBookings(Status);
            //viewModel.TennisCourtModels = _tennisCourtProvider.GetTennisCourtList();
            //viewModel.TennisCourtBookingUserModels = _userProvider.GetUsersList();
            //viewModel.IsStatus = Status;
            return View();
        }
        [HttpPost]
        public JsonResult UserBookingDetails(int status,string searchText)
        
       {

            var model = GetPagingRequestModel();
            var viewModel = _tennisCourtProvider.GetBookingList(model,status,searchText);
            return Json(viewModel);
        }
        //[HttpGet]
        //public IActionResult BookingDetails(string Status)
        //{
        //    TennisCourtBookingViewModel viewModel = new TennisCourtBookingViewModel();
        //    viewModel.TennisCourtBookingModels = _bookingProvider.GetAllBookings(Status);
        //    viewModel.TennisCourtModels = _tennisCourtProvider.GetTennisCourtList();
        //    viewModel.TennisCourtBookingUserModels = _userProvider.GetUsersList();
        //    viewModel.IsStatus= Status;
        //    return View(viewModel);
        //}

        [HttpGet]
        public IActionResult PreviousBookings()
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            TennisCourtBookingViewModel model = new TennisCourtBookingViewModel();
           model.UserBookingDetailsModels =_tennisCourtProvider.ShowPreviousBookings();
            return View(model);
        }
        public TennisCourtBookingViewModel ApiMethod()
        {
            TennisCourtBookingViewModel model = new TennisCourtBookingViewModel();
            model.UserBookingDetailsModels = _tennisCourtProvider.ShowPreviousBookings();
            return model;
        }
        //[HttpPost]
        //public JsonResult ExtraSearch(string searchText)
        //{
        //    var model = GetPagingRequestModel();
        //    var viewModel = _tennisCourtProvider.GetList(model,searchText);
        //    return Json(viewModel);
        //}
        
       
    }
}
