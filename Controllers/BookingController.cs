using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnnualLeave.Controllers
{
    public class BookingController : Controller
    {
        // GET: Booking
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            return Content("hi" + id);
        }


        public ActionResult Option(int? num, string test)
        {
            if (!num.HasValue)
            {
                num = 10;
            }

            if (String.IsNullOrWhiteSpace(test))
            {
                test = "blank";
            }

            return Content(String.Format("id is {0} dBnd sort is {1}", num, test));

        }

        [Route("booking/custom/{name}/{year}")]
        public ActionResult Custom(string name, int year)
        {
            
            return Content(String.Format("id is {0} dBnd sort is {1}", name, year));

        }


    }
}