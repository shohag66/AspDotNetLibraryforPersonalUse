using SelecListUsingTagHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SelecListUsingTagHelper.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            var model = new Employee();
            model.Departments.ToList();
            return View(model);
        }
    }
}