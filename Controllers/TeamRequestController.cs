using AnnualLeave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnnualLeave.Controllers
{
    public class TeamRequestController : Controller
    {
        private ApplicationDbContext _context;

        public TeamRequestController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: TeamRequest
        public ActionResult Index()
        {
            return View();
        }
    }
}