using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Data;

namespace Website.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext db = new DataContext();
        // GET: Cart
        public ActionResult Index()
        {
            var orderDetails = db.OrderDetails;
            return View(orderDetails.ToList());
        }
    }
}