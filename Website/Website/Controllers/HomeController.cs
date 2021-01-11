using PagedList;
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
    public class HomeController : Controller
    {
        private readonly DataContext db = new DataContext();
        // GET: Home
        public ActionResult Index()
        {
            var products = db.Products;
            string Chuoi = "";
            var pro = (from p in db.Products orderby p.ProductID where p.CategoryID == 2 && p.ProductID<=4 select p).Take(4).ToList();
            for (int i = 0; i < pro.Count; i++)
            {
                Chuoi += "<li class=\"span3\">";
                Chuoi += "<div class=\"product-box\">";
                Chuoi += "<span class=\"sale_tag\"></span>";
                Chuoi += "<p><a href=\"Home/ProductDeltal/" + pro[i].ProductID + "\"><img src=\"" + pro[i].Hinh + "\" /></a></p>";
                Chuoi += "<a href=\"Home/ProductDeltal/" + pro[i].ProductID + "\" class=\"title\">" + pro[i].ProductName + "</a><br>";
                Chuoi += "<a href=\"Home/ProductDeltal/" + pro[i].ProductID + "\" class=\"category\">" + pro[i].Option + "</a>";
                Chuoi += " <p class=\"price\">" + pro[i].Price + " VND</p>";
                Chuoi += "</div>";
                Chuoi += "</li>";

            }
            ViewBag.View = Chuoi;

            string Chuoi1 = "";
            var pro1 = (from p in db.Products orderby p.ProductID where p.CategoryID == 2 && p.ProductID >= 5 select p).Take(4).ToList();
            for (int i = 0; i < pro1.Count; i++)
            {
                Chuoi1 += "<li class=\"span3\">";
                Chuoi1 += "<div class=\"product-box\">";
                Chuoi1 += "<span class=\"sale_tag\"></span>";
                Chuoi1 += "<p><a href=\"Home/ProductDeltal/" + pro1[i].ProductID + "\"><img src=\"" + pro1[i].Hinh + "\" /></a></p>";
                Chuoi1 += "<a href=\"Home/ProductDeltal/" + pro1[i].ProductID + "\" class=\"title\">" + pro1[i].ProductName + "</a><br>";
                Chuoi1 += "<a href=\"Home/ProductDeltal/" + pro1[i].ProductID + "\" class=\"category\">" + pro1[i].Option + "</a>";
                Chuoi1 += " <p class=\"price\">" + pro1[i].Price + " VND</p>";
                Chuoi1 += "</div>";
                Chuoi1 += "</li>";

            }
            ViewBag.View1 = Chuoi1;

            string Chuoi2 = "";
            var pro2 = (from p in db.Products orderby p.ProductID where p.CategoryID == 1 && p.ProductID <= 12 select p).Take(4).ToList();
            for (int i = 0; i < pro2.Count; i++)
            {
                Chuoi2 += "<li class=\"span3\">";
                Chuoi2 += "<div class=\"product-box\">";
                Chuoi2 += "<span class=\"sale_tag\"></span>";
                Chuoi2 += "<p><a href=\"Home/ProductDeltal/" + pro2[i].ProductID + "\"><img src=\"" + pro2[i].Hinh + "\" /></a></p>";
                Chuoi2 += "<a href=\"Home/ProductDeltal/" + pro2[i].ProductID + "\" class=\"title\">" + pro2[i].ProductName + "</a><br>";
                Chuoi2 += "<a href=\"Home/ProductDeltal/" + pro2[i].ProductID + "\" class=\"category\">" + pro2[i].Option + "</a>";
                Chuoi2 += " <p class=\"price\">" + pro2[i].Price + " VND</p>";
                Chuoi2 += "</div>";
                Chuoi2 += "</li>";

            }
            ViewBag.View2 = Chuoi2;

            string Chuoi3 = "";
            var pro3 = (from p in db.Products orderby p.ProductID where p.CategoryID == 1 && p.ProductID >= 13 select p).Take(4).ToList();
            for (int i = 0; i < pro3.Count; i++)
            {
                Chuoi3 += "<li class=\"span3\">";
                Chuoi3 += "<div class=\"product-box\">";
                Chuoi3 += "<span class=\"sale_tag\"></span>";
                Chuoi3 += "<p><a href=\"Home/ProductDeltal/" + pro3[i].ProductID + "\"><img src=\"" + pro3[i].Hinh + "\" /></a></p>";
                Chuoi3 += "<a href=\"Home/ProductDeltal/" + pro3[i].ProductID + "\" class=\"title\">" + pro3[i].ProductName + "</a><br>";
                Chuoi3 += "<a href=\"Home/ProductDeltal/" + pro3[i].ProductID + "\" class=\"category\">" + pro3[i].Option + "</a>";
                Chuoi3 += " <p class=\"price\">" + pro3[i].Price + " VND</p>";
                Chuoi3 += "</div>";
                Chuoi3 += "</li>";

            }
            ViewBag.View3 = Chuoi3;
            return View(products.ToList());
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Product(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            int pageSize = 9;
            int pageNumber = (page ?? 1);

            ViewBag.MaSortParm = String.IsNullOrEmpty(sortOrder) ? "Ma_desc" : "";
            ViewBag.MaSortParm = String.IsNullOrEmpty(sortOrder)  ? "" : "Loai2";
            ViewBag.MaSortParm = String.IsNullOrEmpty(sortOrder) ? "" : "Loai2";
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.Loai1SortParm = sortOrder == "Loai1" ? "Loai1_desc" : "Loai1";
            ViewBag.Loai2SortParm = sortOrder == "Loai2" ? "Loai2_desc" : "Loai2";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            var products = from s in db.Products
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName.Contains(searchString)
                                       || s.Mota.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Ma_desc":
                    products = products.OrderByDescending(s => s.CategoryID);
                    break;
                case "Name":
                    products = products.OrderBy(s => s.ProductName);
                    break;
                case "Name_desc":
                    products = products.OrderByDescending(s => s.ProductName);
                    break;
                case "Loai1":
                    products = products.OrderBy(s => s.CategoryID);
                    products = products.Where(s => s.CategoryID == 2);
                    break;
                case "Loai2":
                    products = products.OrderBy(s => s.CategoryID);
                    products = products.Where(s => s.CategoryID == 1);
                    break;
                case "Price":
                    products = products.OrderBy(s => s.Price);
                    break;
                case "Price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
                default:
                    products = products.OrderBy(s => s.CategoryID);
                    break;
            }

            string Chuoi2 = "";
            var pro2 = (from p in db.Products orderby p.ProductID where p.ProductID == 6 select p).Take(1).ToList();
            for (int i = 0; i < pro2.Count; i++)
            {
                Chuoi2 += "<ul class=\"thumbnails listing-products\">";
                Chuoi2 += "<li class=\"span3\">";
                Chuoi2 += "<div class=\"product-box\">";
                Chuoi2 += "<span class=\"sale_tag\"></span>";
                Chuoi2 += "<a href=\"ProductDeltal/" + pro2[i].ProductID + "\"><img src=\"" + pro2[i].Hinh + "\"/></a><br/>";
                Chuoi2 += "<a href=\"ProductDeltal/" + pro2[i].ProductID + "\" class=\"title\">" + pro2[i].ProductName + "</a><br/>";
                Chuoi2 += "<a href=\"ProductDeltal/" + pro2[i].ProductID + "\" class=\"category\">" + pro2[i].Option + "</a>";
                Chuoi2 += "<p class=\"price\">" + pro2[i].Price + " VND</p>";
                Chuoi2 += "</div>";
                Chuoi2 += "</li>";
                Chuoi2 += "</ul>";
            }
            ViewBag.View2 = Chuoi2;

            string Chuoi3 = "";
            var pro3 = (from p in db.Products orderby p.ProductID where p.ProductID == 11 select p).Take(1).ToList();
            for (int i = 0; i < pro3.Count; i++)
            {
                Chuoi3 += "<ul class=\"thumbnails listing-products\">";
                Chuoi3 += "<li class=\"span3\">";
                Chuoi3 += "<div class=\"product-box\">";
                Chuoi3 += "<span class=\"sale_tag\"></span>";
                Chuoi3 += "<a href=\"ProductDeltal/" + pro3[i].ProductID + "\"><img src=\"" + pro3[i].Hinh + "\"/></a><br/>";
                Chuoi3 += "<a href=\"ProductDeltal/" + pro3[i].ProductID + "\" class=\"title\">" + pro3[i].ProductName + "</a><br/>";
                Chuoi3 += "<a href=\"ProductDeltal/" + pro3[i].ProductID + "\" class=\"category\">" + pro3[i].Option + "</a>";
                Chuoi3 += "<p class=\"price\">" + pro3[i].Price + " VND</p>";
                Chuoi3 += "</div>";
                Chuoi3 += "</li>";
                Chuoi3 += "</ul>";
            }
            ViewBag.View3 = Chuoi3;
            string Chuoi4 = "";
            var pro4 = (from p in db.Products orderby p.ProductID where p.Price >= 80000000 select p).Take(3).ToList();
            for (int i = 0; i < pro4.Count; i++)
            {
                Chuoi4 += "<li >";
                Chuoi4 += "<a href=\"/ProductDeltal/" + pro4[i].ProductID + "/\" title = \"" + pro4[i].ProductName + "\" >";
                Chuoi4 += "<img src = \" " + pro4[i].Hinh + "\" alt = \"#\" >";
                Chuoi4 += "</a >";
                Chuoi4 += "<a  href=\"/ProductDeltal/" + pro4[i].ProductID + "/\" >" + pro4[i].Option + "</ a >";
                Chuoi4 += "</li >";
            }
            ViewBag.View4 = Chuoi4;
            return View(products.ToPagedList(pageNumber, pageSize));
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
            string Chuoi = "";
            var pro = (from p in db.Products orderby p.ProductID select p).Take(3).ToList();
            for (int i = 0; i < pro.Count; i++)
            {
                Chuoi += "<li class=\"span3\">";
                Chuoi += "<div class=\"product-box\">";
                Chuoi += "<span class=\"sale_tag\"></span>";
                Chuoi += "<a href=\"" + pro[i].ProductID + "\"><img src=\"" + pro[i].Hinh + "\"/></a><br/>";
                Chuoi += "<a href=\"" + pro[i].ProductID + "\" class=\"title\">" + pro[i].ProductName + "</a><br/>";
                Chuoi += "<a href=\"" + pro[i].ProductID + "\" class=\"category\">" + pro[i].Option + "</a>";
                Chuoi += "<p class=\"price\">" + pro[i].Price + " VND</p>";
                Chuoi += "</div>";
                Chuoi += "</li>";
            }
            ViewBag.View = Chuoi;

            string Chuoi1 = "";
            var pro1 = (from p in db.Products orderby p.ProductID descending select p).Take(3).ToList();
            for (int i = 0; i < pro1.Count; i++)
            {
                Chuoi1 += "<li class=\"span3\">";
                Chuoi1 += "<div class=\"product-box\">";
                Chuoi1 += "<span class=\"sale_tag\"></span>";
                Chuoi1 += "<a href=\"" + pro1[i].ProductID + "\"><img src=\"" + pro1[i].Hinh + "\"/></a><br/>";
                Chuoi1 += "<a href=\"" + pro1[i].ProductID + "\" class=\"title\">" + pro1[i].ProductName + "</a><br/>";
                Chuoi1 += "<a href=\"" + pro1[i].ProductID + "\" class=\"category\">" + pro1[i].Option + "</a>";
                Chuoi1 += "<p class=\"price\">" + pro1[i].Price + " VND</p>";
                Chuoi1 += "</div>";
                Chuoi1 += "</li>";
            }
            ViewBag.View1 = Chuoi1;

            string Chuoi2 = "";
            var pro2 = (from p in db.Products orderby p.ProductID where p.ProductID == 6 select p).Take(1).ToList();
            for (int i = 0; i < pro2.Count; i++)
            {
                Chuoi2 += "<ul class=\"thumbnails listing-products\">";
                Chuoi2 += "<li class=\"span3\">";
                Chuoi2 += "<div class=\"product-box\">";
                Chuoi2 += "<span class=\"sale_tag\"></span>";
                Chuoi2 += "<a href=\"" + pro2[i].ProductID + "\"><img src=\"" + pro2[i].Hinh + "\"/></a><br/>";
                Chuoi2 += "<a href=\"" + pro2[i].ProductID + "\" class=\"title\">" + pro2[i].ProductName + "</a><br/>";
                Chuoi2 += "<a href=\"" + pro2[i].ProductID + "\" class=\"category\">" + pro2[i].Option + "</a>";
                Chuoi2 += "<p class=\"price\">" + pro2[i].Price + " VND</p>";
                Chuoi2 += "</div>";
                Chuoi2 += "</li>";
                Chuoi2 += "</ul>";
            }
            ViewBag.View2 = Chuoi2;

            string Chuoi3 = "";
            var pro3 = (from p in db.Products orderby p.ProductID where p.ProductID == 11 select p).Take(1).ToList();
            for (int i = 0; i < pro3.Count; i++)
            {
                Chuoi3 += "<ul class=\"thumbnails listing-products\">";
                Chuoi3 += "<li class=\"span3\">";
                Chuoi3 += "<div class=\"product-box\">";
                Chuoi3 += "<span class=\"sale_tag\"></span>";
                Chuoi3 += "<a href=\"" + pro3[i].ProductID + "\"><img src=\"" + pro3[i].Hinh + "\"/></a><br/>";
                Chuoi3 += "<a href=\"" + pro3[i].ProductID + "\" class=\"title\">" + pro3[i].ProductName + "</a><br/>";
                Chuoi3 += "<a href=\"" + pro3[i].ProductID + "\" class=\"category\">" + pro3[i].Option + "</a>";
                Chuoi3 += "<p class=\"price\">" + pro3[i].Price + " VND</p>";
                Chuoi3 += "</div>";
                Chuoi3 += "</li>";
                Chuoi3 += "</ul>";
            }
            ViewBag.View3 = Chuoi3;
            string Chuoi4 = "";
            var pro4 = (from p in db.Products orderby p.ProductID where p.Price >= 80000000 select p).Take(3).ToList();
            for (int i = 0; i < pro4.Count; i++)
            {
                Chuoi4 += "<li >";
                Chuoi4 += "<a href=\"" + pro4[i].ProductID + "\" title =\"" + pro4[i].ProductName + "\" >";
                Chuoi4 += "<img src =\"" + pro4[i].Hinh + "\" alt = \"#\" >";
                Chuoi4 += "</a >";
                Chuoi4 += "<a  href=\"" + pro4[i].ProductID + "\" >" + pro4[i].Option + "</ a >";
                Chuoi4 += "</li >";
            }
            ViewBag.View4 = Chuoi4;
            return View(product);
        }
    }
}