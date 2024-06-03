using Microsoft.AspNetCore.Mvc;
using TennisCourtBookingApp.Common.CommonEntities;
using TennisCourtBookingApp.Provider.IProvider;
using TennisCourtBookingApp.Provider.Provider;
using WebApplication1.Models;
using TennisCourtBookingApp.Provider.Mapping;
using AutoMapper;
using TennisCourtBookingApp.Web.Controllers;
using TennisCourtBookingApp.Web.Controllers;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class BookingDetailsController : Controller
    {
      
        public BookingDetailsController( )
        {
           
        }



        [HttpPost("PreviousBookings")]
        public IActionResult PreviousBookings()
        {
          

              var query = "this is demo test for api";
            return Ok(query);
        }
    }
}
