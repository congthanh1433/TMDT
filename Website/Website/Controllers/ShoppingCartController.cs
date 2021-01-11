using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Data;
using Website.Models;
using Website.Models.FPayPal;

namespace Website.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly DataContext db = new DataContext();   
        // GET: ShoppingCart
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if ((cart == null) || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public ActionResult AddtoCart(int id)
        {
            var pro = db.Products.SingleOrDefault(s => s.ProductID == id);
            if (pro != null)
            {
                GetCart().Add(pro);
            }
            return RedirectToAction("ShowtoCart", "ShoppingCart");
        }
        public ActionResult ShowtoCart()    
        {
            if (Session["Cart"] == null)
                return RedirectToAction("ShowtoCart", "ShoppingCart");
            Cart cart = Session["Cart"] as Cart;
            return View(cart);
        }
        public ActionResult Update_Quantity_Cart(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["ID_Product"]);
            int quantity = int.Parse(form["Quantity"]);
            cart.Update_Quantity_Shopping(id_pro, quantity);
            return RedirectToAction("ShowtoCart", "ShoppingCart");
        }
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowToCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int total_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)

                total_item = cart.Total_Quantity_in_Cart();
            ViewBag.QuantityCart = total_item;
            return PartialView("BagCart");

        }
        public ActionResult Shopping_Success()
        {
            return View();
        }
        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                Models.Order order = new Models.Order
                {
                    Date = DateTime.Now,
                    PhoneNumber = form["PhoneNumber"],
                    Descriptions = form["Address_Delivery"],
                    Email = form["Email"],

                    TinhTrang = "Dang cho",
                    CustomerID = null,
                    PhuongThuc = "Nolmal",
            };
                db.Orders.Add(order);
                db.SaveChanges();
                foreach (var item in cart.Items)
                {
                    OrderDetail order_Detail = new OrderDetail
                    {
                        OrderID = order.OrderID,
                        ProductID = item.Shopping_product.ProductID,
                        UnitPriceSale = item.Shopping_product.Price,
                        QuantitySale = item.Shopping_quantity
                    };
                    db.OrderDetails.Add(order_Detail);
                }
                db.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("Shopping_Success", "ShoppingCart");
            }
            catch
            {
                return Content("Error Checkout. Please infomation of Customer...");
            }
        }
        private Payment payment;
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var listItems = new ItemList() { items = new List<PayPal.Api.Item>() };
          
            Cart listCart = Session["Cart"] as Cart;
            foreach (var cart in listCart.Items)
            {
                listItems.items.Add(new PayPal.Api.Item()
                {
                    name = cart.Shopping_product.ProductName,
                    currency = "USD",
                    price = cart.Shopping_product.Price.ToString(),
                    quantity = cart.Shopping_quantity.ToString(),
                    sku = "sku"
                });
            }

            var payer = new Payer() { payment_method = "paypal" };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            var details = new Details()
            {
                tax = "1",
                shipping = "2",
                subtotal = listCart.Items.Sum(x => x.Shopping_quantity * x.Shopping_product.Price).ToString()
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(),
                details = details
            };

            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Hello test thôi",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = listItems
            });

            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return payment.Create(apiContext);
        }
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        public ActionResult PaymentWithPaypal()
        {
            Cart cart = Session["Cart"] as Cart;

            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/ShoppingCart/PaymentWithPaypal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = string.Empty;

                    while (links.MoveNext())
                    {
                        Links link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = link.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executePayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    if (executePayment.state.ToLower() != "approved")
                    {
                        cart.ClearCart();
                        return View("Failure");
                    }
                }
            }
            catch (Exception e)
            {
                PaypalLogger.Log("Error: " + e.Message);
                cart.ClearCart();
                return View("Failure");
            }
            if (User.Identity.IsAuthenticated)
            {
                //Orders
                var acc = db.Orders.FirstOrDefault(x => x.Customer.Email == User.Identity.Name);
                var order = new Models.Order();
                order.Date = DateTime.Now;
                order.TinhTrang = "Dang cho";
                order.CustomerID = acc.CustomerID;
                order.PhoneNumber = acc.PhoneNumber;
                order.Email = acc.Email;
                order.PhuongThuc = "PayPal";
                order.Descriptions = "";
                db.Orders.Add(order);
                db.SaveChanges();

                //OrderDetail
                foreach (var item in cart.Items)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {

                        QuantitySale = item.Shopping_quantity,
                        UnitPriceSale = item.Shopping_quantity * item.Shopping_product.Price,
                        OrderID = order.OrderID,
                        ProductID = item.Shopping_product.ProductID,
                    };
                    db.OrderDetails.Add(orderDetail);
                    db.SaveChanges();

                    //giảm số lượng sp sau khi đặt
                    var product = db.Products.FirstOrDefault(x => x.ProductID == item.Shopping_product.ProductID);
                    product.Quantity = product.Quantity - item.Shopping_quantity;
                    db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {

                var order = new Models.Order();
                order.Date = DateTime.Now;
                order.TinhTrang = "Dang cho";
                order.CustomerID = null;
                order.PhoneNumber = "123";
                order.Email = "123";
                order.PhuongThuc = "PayPal";
                order.Descriptions = "";
                db.Orders.Add(order);
                db.SaveChanges();

                //OrderDetail
                foreach (var item in cart.Items)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {

                        QuantitySale = item.Shopping_quantity,
                        UnitPriceSale = item.Shopping_quantity * item.Shopping_product.Price,
                        OrderID = order.OrderID,
                        ProductID = item.Shopping_product.ProductID,
                    };
                    db.OrderDetails.Add(orderDetail);
                    db.SaveChanges();

                    //giảm số lượng sp sau khi đặt
                    var product = db.Products.FirstOrDefault(x => x.ProductID == item.Shopping_product.ProductID);
                    product.Quantity = product.Quantity - item.Shopping_quantity;
                    db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            cart.ClearCart();
            return View("Success");
        }
    }
}