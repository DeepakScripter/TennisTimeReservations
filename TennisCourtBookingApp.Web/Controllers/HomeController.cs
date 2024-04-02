using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TennisCourtBookingApp.Common.CommonEntities;
using System.Diagnostics;
using TennisCourtBookingApp.Common.BusinessEntities;
using TennisCourtBookingApp.Common.Utility;
using TennisCourtBookingApp.Provider.IProvider;
using TennisCourtBookingApp.Web.Models;

namespace TennisCourtBookingApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICommonProvider _commonProvider;
        private readonly IUserProvider _userProvider;
        private IWebHostEnvironment _webHostEnvironment;

        public HomeController(ICommonProvider commonProvider, IUserProvider userProvider, IWebHostEnvironment webHostEnvironment)
        {
            _commonProvider = commonProvider;
            _webHostEnvironment = webHostEnvironment;
            _userProvider = userProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {

            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                return RedirectToAction("TennisCourt", "Index");
            }
            else if (HttpContext.Session.GetString("UserSession") != null)
            {
                int? userId = HttpContext.Session.GetInt32("UserId");
                return RedirectToAction("UserDetails", "User", new { userId = userId });
            }

            return PartialView("_UserLogin");
        }


        [HttpPost]
        public IActionResult Login(TennisCourtBookingViewModel loginDetails)
        {

            var details = _commonProvider.FindRole(loginDetails.TennisCourtBookingUserModel);
            if (details != null)
            {
                if (details.RoleName == "Admin")
                {
                    HttpContext.Session.SetString("AdminSession", "Login");
                    return Json(details.RoleName);

                }
                else
                {
                    HttpContext.Session.SetString("UserSession", "Login");
                    int userId = _commonProvider.FindUser(loginDetails.TennisCourtBookingUserModel);
                    HttpContext.Session.SetInt32("UserId", userId);
                    //return RedirectToAction("UserDetails", "User", new { userId = userId });
                    //return Json(userId);
                    return RedirectToAction("UserDetails", "User", new { userId = userId });
                }
            }
            else
            {
                ViewBag.Message = "Login Failed..";
            }
            return View();
        }
        [HttpGet]
        public IActionResult UserSignUp()
        {
            TennisCourtBookingViewModel model = new TennisCourtBookingViewModel();
            model.IsEdit = false;
            model.IsUser = true;
            return PartialView("_UserPartial", model);
        }

        //[HttpPost]
        //public IActionResult UserSignUp(TennisCourtBookingViewModel signupDetails)
        //{
        //    _commonProvider.RegisterUser(signupDetails.TennisCourtBookingUserModel);
        //    return Json("");

        //}

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Index");
            }
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                HttpContext.Session.Remove("AdminSession");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
       

        [HttpPost]
        public IActionResult SignUp(TennisCourtBookingViewModel inputModel)
        {
            TennisCourtBookingViewModel model = new TennisCourtBookingViewModel();
            if (inputModel.Image != null && inputModel.Image.Length > 0)
            {
                //changing the file name
                string fileName = Path.GetFileNameWithoutExtension(inputModel.Image.FileName);
                string prfileName = Guid.NewGuid().ToString() + "__" + fileName + Path.GetExtension(inputModel.Image.FileName);
                FileInfo fi = new FileInfo(prfileName);
                //Checking if the file is image or not
                if (fi.Extension.ToLower() == ".image" || fi.Extension.ToLower() == ".png" || fi.Extension.ToLower() == ".jpg" || fi.Extension.ToLower() == ".jpeg")
                {
                    var rootPath = _webHostEnvironment.WebRootPath;
                    //Checking the path to save the image
                    string documentFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ExtraFolder\\ProfileImage");
                   
                    if (!Directory.Exists(documentFolder))
                        Directory.CreateDirectory(documentFolder);

                    string prfilePath = Path.Combine(documentFolder, prfileName);
                    using (var ms = new FileStream(prfilePath, FileMode.Create))
                    {
                        inputModel.Image.CopyTo(ms);
                    }
                    //string Imagee = prfileName;
                   
                    //model = _userProvider.SaveEmployeeProfileImg(inputModel.TennisCourtBookingUserModel);
                    int? userId = HttpContext.Session.GetInt32("UserId");
                    if(userId!=null)
                    {
                        inputModel.TennisCourtBookingUserModel = new TennisCourtBookingUserModel();
                        inputModel.TennisCourtBookingUserModel.Image = prfileName;
                        _userProvider.UpdateUserImage(inputModel.TennisCourtBookingUserModel, userId);
                    }
                    else {
                        inputModel.TennisCourtBookingUserModel.Image = prfileName;
                        _commonProvider.RegisterUser(inputModel.TennisCourtBookingUserModel);
                    }
                    //_commonProvider.SaveImage(userId, Imagee);
                }
                else
                {
                    model.IsSuccess = false;
                    model.Message = "You can only upload image or jpg files";
                }
            }
            return Json("");
        }
    }
}