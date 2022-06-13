using Shopping_Website_2020.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopping_Website_2020.Controllers
{
    public class UserController : Controller
    {
        shopping_website_DBEntities db = new shopping_website_DBEntities();
        // GET: User
        public ActionResult Index()
        {
            try
            {
                Response.Cache.SetNoStore();
                Session["uemail"] = Session["email"].ToString();
                Session["fullnamee"] = Session["fullname"].ToString();
            }
            catch (Exception ex)
            {
                ViewBag.s = ex.ToString();
                return RedirectToAction("Customer_Login", "User");
            }
            
            return View();
        }

        public ActionResult Customer_Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Customer_Registration(tb_user tu)
        {
            try
            {
                tb_user u = new tb_user();
                u.fullname = Request.Form["fullname"];
                u.email = Request.Form["email"];
                u.password = Request.Form["password"];
                u.entry_date = DateTime.Now;
                db.tb_user.Add(u);
                db.SaveChanges();
                return RedirectToAction("Customer_Login","User");
            }
            catch (Exception ex)
            {
                ViewBag.s = ex.ToString();
                return RedirectToAction("Customer_Registration", "User");
            }
        }
        public ActionResult Customer_Login()
        {
            try
            {
                Response.Cache.SetNoStore();
                if (Session["email"].ToString()!=null)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ViewBag.s = ex.ToString();
                return View();

            }
            
        }
        [HttpPost]
        public ActionResult Customer_Login(tb_user t)
        {
            try
            {
                var getlogin = db.tb_user.Where(x => x.email == t.email && x.password == t.password).SingleOrDefault();
                if(getlogin!=null)
                {
                    Session["uid"] = getlogin.id.ToString();
                    Session["fullname"] = getlogin.fullname.ToString();
                    Session["email"] = getlogin.email.ToString();
                    Session["pass"] = getlogin.password.ToString();

                    return RedirectToAction("Index", "User");
                }
                else
                {
                    TempData["Error"] = "Invalid Login";
                    return RedirectToAction("Customer_Login","User");
                }
            }
            catch (Exception ex)
            {
                ViewBag.s = ex.ToString();
                return RedirectToAction("Customer_Login","User");
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }

        public ActionResult user_info()
        {
            return View();
        }

        public ActionResult order_info()
        {
            var getuid = Convert.ToInt32(Session["uid"].ToString());
            var getOrder_detail = db.tb_order.Where(x => x.userID == getuid).ToList();
            var getOrder_Amount_Sum = db.tb_order.Where(x => x.userID == getuid).Sum(x=>x.o_unitPrice);
            ViewBag.ShowOrderAmount = getOrder_Amount_Sum;
            ViewBag.showOrder = getOrder_detail;
            return View();
        }



    

    }
}