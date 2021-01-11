using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Data;

namespace Website.Controllers
{
    public class LayoutController : Controller
    {
        private readonly DataContext db = new DataContext();
        // GET: Layout
        public ActionResult HomeLayout()
        {
            return View();
        }
        public ActionResult Search(string name)
        {
            var result = db.Products.Where(x => x.ProductName.Contains(name)).ToList();
            return View(result);

        }
    }
}