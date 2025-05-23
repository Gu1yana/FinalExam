﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Moderator, SuperAdmin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
