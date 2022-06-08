using InterviewWebApplication.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InterviewWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private ConnectionFactory _connectionFactory;

        public HomeController()
        {
            _connectionFactory = new ConnectionFactory();
        }

        public ActionResult Index()
        {
            return View();
        }

        
    }
}