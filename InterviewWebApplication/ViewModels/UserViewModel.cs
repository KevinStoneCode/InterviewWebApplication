using InterviewWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewWebApplication.ViewModels
{
    public class UserViewModel
    {
        public IEnumerable<User> Users { get; set; }
    }
}