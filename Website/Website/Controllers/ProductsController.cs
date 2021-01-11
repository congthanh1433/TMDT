using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Website.Data;
using Website.Models;

namespace Website.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DataContext db = new DataContext();
        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products;
            return View(products.ToList());
        }
        public ActionResult ProductDeltal()
        {
            string Chuoi = "";
            var product = (from p in db.Products orderby p.ProductID select p).Take(3).ToList();
            for (int i = 0; i < product.Count; i++)
            {
                Chuoi += "<li class=\"span3\">";
                Chuoi += "<div class=\"product-box\">";
                Chuoi += "<span class=\"sale_tag\"></span>";
                Chuoi += "<a href=\"#\"><img src=\"" + product[i].Hinh + "\"/></a><br/>";
                Chuoi += "<a href=\"http://localhost:63199/ProductsDefault/index\" class=\"title\">" + product[i].ProductName + "</a><br/>";
                Chuoi += "<a href=\"http://localhost:63199/ProductsDefault/Index\" class=\"category\">" + product[i].Option + "</a>";
                Chuoi += "<p class=\"price\">" + product[i].Price + " VND</p>";
                Chuoi += "</div>";
                Chuoi += "</li>";
            }
            ViewBag.View = Chuoi;

            string Chuoi1 = "";
            var product1 = (from p in db.Products orderby p.ProductID descending select p).Take(3).ToList();
            for (int i = 0; i < product.Count; i++)
            {
                Chuoi1 += "<li class=\"span3\">";
                Chuoi1 += "<div class=\"product-box\">";
                Chuoi1 += "<span class=\"sale_tag\"></span>";
                Chuoi1 += "<a href=\"#\"><img src=\"" + product1[i].Hinh + "\"/></a><br/>";
                Chuoi1 += "<a href=\"http://localhost:63199/ProductsDefault/index\" class=\"title\">" + product1[i].ProductName + "</a><br/>";
                Chuoi1 += "<a href=\"http://localhost:63199/ProductsDefault/Index\" class=\"category\">" + product1[i].Option + "</a>";
                Chuoi1 += "<p class=\"price\">" + product1[i].Price + " VND</p>";
                Chuoi1 += "</div>";
                Chuoi1 += "</li>";
            }
            ViewBag.View1 = Chuoi1;
            return View();
        }
        public ActionResult ProductDeltal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

    }
}