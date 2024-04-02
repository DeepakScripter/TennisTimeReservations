using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisCourtBookingApp.Common.Utility
{
    public class AppCommon
    {


        public static string ErrorMessage = "Something Went Wrong. Please Contact Administrator!";
        public static string ApplicationLongTitle = "DemoProjetCRUD";
        public static string ApplicationTitle = "DemoProjectCRUD";
        public static string Protection = "DemoProjectCRUD";
        public static string ConnectionString = "";
        public static String Trim = "";
        public static DateTime CurrentDate
        {
            get
            {
                return DateTime.Now;

            }
        }
    }
}
