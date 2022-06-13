using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Shopping_Website_2020.Models;
using System.Data.Entity;

namespace Shopping_Website_2020.Controllers
{
    public class AdminController : Controller
    {
        shopping_website_DBEntities db = new shopping_website_DBEntities();
        // GET: Admin
        public ActionResult Index()
        {
            try
            {
                Response.Cache.SetNoStore();
                Session["adminname"] = Session["fullnameAdmin"].ToString();
            }
            catch (Exception ex)
            {
                ViewBag.f = ex.ToString();
                return RedirectToAction("Login","Admin");
            }
            return View();
        }

        public ActionResult Login()
        {
            try
            {
                Response.Cache.SetNoStore();
                if (Session["fullnameAdmin"].ToString() != null)
                {
                    return RedirectToAction("Index", "Admin");
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
        public ActionResult SetLogin(tb_admin t)
        {

            try
            {
                var getlogin = db.tb_admin.Where(x => x.Username == t.Username && x.password == t.password).SingleOrDefault();
                if (getlogin != null)
                {
                    Session["adminid"] = getlogin.id.ToString();
                    Session["fullnameAdmin"] = getlogin.Username.ToString();
                    


                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    TempData["Error"] = "Invalid Login";
                    return RedirectToAction("Login", "Admin");
                }
            }
            catch (Exception ex)
            {
                ViewBag.s = ex.ToString();
                return RedirectToAction("Login", "Admin");
            }
        }
        public ActionResult AddNewCatgeory()
        {
           if(Session["fullnameAdmin"]!=null)
            {
                return View();
            }
           else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult ClickSaveCatefory()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tb_category c = new tb_category();
                    c.category_name = Request.Form["category_name"];
                    c.entry_date = DateTime.Now;
                    db.tb_category.Add(c);
                    db.SaveChanges();
                    TempData["Message"] = "Catgegory Save.";
                    return RedirectToAction("AddNewCatgeory", "Admin");
                }
                else
                {
                    return RedirectToAction("AddNewCatgeory", "Admin");
                }
            }
            catch (Exception ex)
            {
                ViewBag.s = ex.ToString();
                return RedirectToAction("AddNewCatgeory", "Admin");
            }
        }

        
        public ActionResult ViewCatgeory()
        {
            if (Session["fullnameAdmin"] != null)
            {
                var getall_catgory = db.tb_category.ToList();
                return View(getall_catgory);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }

        public ActionResult Edit(int id)
        {
            var getedit = db.tb_category.Where(x => x.c_id == id).FirstOrDefault();
            return View(getedit);
        }
        [HttpPost]

        public ActionResult Edit(tb_category tc)
        {
            if(ModelState.IsValid)
            {
                db.Entry(tc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            if(id>0)
            {
                var getbyid = db.tb_category.Where(x=>x.c_id==id).FirstOrDefault();
                if (getbyid != null) {
                    db.Entry(getbyid).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult AddNewBrand()
        {
            if (Session["fullnameAdmin"] != null)
            {
               
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult SetAddNewBrand()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tb_brand b = new tb_brand();
                    b.brand_name = Request.Form["brand_name"];
                    b.entry_date = DateTime.Now;
                    db.tb_brand.Add(b);
                    db.SaveChanges();
                    TempData["Message"] = "Brand Save";
                    return RedirectToAction("AddNewBrand", "Admin");
                }
                else
                {
                    return RedirectToAction("AddNewBrand", "Admin");
                }
            }
            catch (Exception ex)
            {
                ViewBag.s = ex.ToString();
                return RedirectToAction("AddNewBrand", "Admin");
            }
        }

        public ActionResult ViewBrand()
        {
            if (Session["fullnameAdmin"] != null)
            {
                var getall_brand = db.tb_brand.ToList();
                return View(getall_brand);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        public ActionResult AddNewproduct()
        {
            if (Session["fullnameAdmin"] != null)
            {
                ViewBag.showCategory = new SelectList(db.tb_category, "c_id", "category_name");
                ViewBag.showBrand = new SelectList(db.tb_brand, "b_id", "brand_name");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

           
        }
        public ActionResult SetAddNewproduct(tb_product tbpro, HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    //image save to image folder 
                    string imagename = System.IO.Path.GetFileName(file.FileName);
                    string physicalpath = Server.MapPath("~/Product_images/" + imagename);
                    file.SaveAs(physicalpath);

                    //Save on Database
                    tb_product t = new tb_product();
                    t.product_name = Request.Form["product_name"];
                    t.product_price = Convert.ToInt32(Request.Form["product_price"].ToString());
                    t.product_detail = Request.Form["product_detail"];
                    t.category_id = Convert.ToInt32(Request.Form["category_id"].ToString());
                    t.brand_id = Convert.ToInt32(Request.Form["brand_id"].ToString());
                    t.product_image = imagename;
                    t.entry_date = DateTime.Now;
                    db.tb_product.Add(t);
                    db.SaveChanges();
                    TempData["Message"] = "Product Save";
                }
                return RedirectToAction("AddNewproduct", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.s = ex.ToString();
                return RedirectToAction("AddNewproduct", "Admin");
            }
        }

        public ActionResult Viewproduct()
        {
            if (Session["fullnameAdmin"] != null)
            {
                var getallPro = db.tb_product.ToList();
                return View(getallPro);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }

        public ActionResult user_info()
        {
            if (Session["fullnameAdmin"] != null)
            {
                var get_all_user = db.tb_user.ToList();
                return View(get_all_user);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }

        public ActionResult Sales_report()
        {
            if (Session["fullnameAdmin"] != null)
            {
                var get_all_sales = db.tb_order.ToList();
                return View(get_all_sales);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
          
        }
    }
}