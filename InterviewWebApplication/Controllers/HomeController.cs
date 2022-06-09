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

        public ActionResult Edit(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserFormViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }
                UpdateUser(viewModel.User);

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                DeleteUser(id);
            }
            catch (Exception ex)
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

            using (var conn = _connectionFactory.CreateConnection())
            {
                var insertedUser = conn.QuerySingleOrDefault<User>(sql, user);
                return insertedUser;
            }
        }

        private void UpdateUser(User user)
        {
            var conn = _connectionFactory.CreateConnection();
            var sql = @"
                    UPDATE Users SET
                        Name = @Name, 
                        Email = @EMail
                    WHERE Id = @Id 
                    ";
            try
            {
                conn.Execute(sql, user);
            }
            catch (Exception ex)
            {
            }
        }

        private void DeleteUser(int id)
        {
            var conn = _connectionFactory.CreateConnection();
            var sql = @"
                    DELETE FROM Users
                    WHERE Id = @Id 
                    ";

            DynamicParameters p = new DynamicParameters();
            p.Add("@Id", id);

            try
            {
                conn.Execute(sql, p);
            }
            catch (Exception ex)
            {
            }
        }
    }
}