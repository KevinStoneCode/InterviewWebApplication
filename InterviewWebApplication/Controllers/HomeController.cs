using Dapper;
using InterviewWebApplication.Connection;
using InterviewWebApplication.Models;
using InterviewWebApplication.ViewModels;
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
            var viewModel = new UserViewModel
            {
                Users = GetUsers()
            };

            return View(viewModel);
        }


        private IEnumerable<User> GetUsers()
        {
            var sql = @"
                    SELECT * FROM Users
                    ";

            using (var conn = _connectionFactory.CreateConnection())
            {
                var users = conn.Query<User>(sql);
                return users;
            }
            
        }
    }
}