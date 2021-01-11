using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Website.Data;
using Website.Models;

namespace Website.Controllers
{
    public class LoginController : Controller
    {
        DataContext db = new DataContext();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register([Bind(Include = "UserID,FirstName,LastName,Email,Password,MaPQ")] User _user)
        {
            if (ModelState.IsValid)
            {
                var sp = db.Users.ToList();

                //Kiểm tra danh sách nếu danh sách rỗng tài khoản đầu tiên sẽ là admin
                if(sp.Count==0)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    _user.MaPQ = 1;
                    db.Users.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                var check = db.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)//nếu email đang sử dụng chưa được đăng kí trước đó
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    _user.MaPQ = 2;
                    db.Users.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Email already exists!Use another email please!";
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        //Dang nhap
        public ActionResult LoginAcc()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAcc(string username, string password)
        {
            var tk = db.Users.Where(p => p.Email == username && p.Password == password).FirstOrDefault();
            if (username == tk.Email && password == tk.Password)
            {
                Session["tk"] = tk;
                FormsAuthentication.SetAuthCookie(username, false);
                return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Index", "Home");

        }
        //logout
        public ActionResult Logout()
        {
            Session["tk"] = null;
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
        public ActionResult acount()
        {
            return View(db.Customers.ToList());
        }
    }
}