﻿using Dapper;
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

        public ActionResult Details(int? id)
        {
            var user = GetUser(id);
            if (user == null)
                return HttpNotFound();

            var viewModel = new UserFormViewModel
            {
                User = user
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            var viewModel = new UserFormViewModel();
            
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserFormViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                CreateUser(viewModel.User);
            }
            catch(Exception ex)
            {

            }
            
            return RedirectToAction("Index");
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

        private User GetUser(int? id)
        {
            var sql = @"
                    SELECT * FROM Users WHERE Id = @Id
                    ";

            DynamicParameters p = new DynamicParameters();
            p.Add("@Id", id);

            using (var conn = _connectionFactory.CreateConnection())
            {
                var user = conn.QuerySingleOrDefault<User>(sql,p);
                return user;
            }

        }

        private User CreateUser(User user)
        {
            var sql = @"
                    INSERT INTO Users (
                        Name, Email
                    )
                    OUTPUT INSERTED.Id
                    VALUES (
                        @Name, @Email
                    )
                    ";

            DynamicParameters p = new DynamicParameters();
            p.Add("@Name", user.Name);
            p.Add("@Email", user.Email);

            using (var conn = _connectionFactory.CreateConnection())
            {
                var insertedUser = conn.QuerySingleOrDefault<User>(sql, p);
                return insertedUser;
            }

        }
    }
}