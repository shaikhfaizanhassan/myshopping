using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopping_Website_2020.Models;

namespace Shopping_Website_2020.Controllers
{
    public class CartsController : Controller
    {
        shopping_website_DBEntities db = new shopping_website_DBEntities();
        // GET: Carts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product_info(int id)
        {
            var Get_single_pro = db.tb_product.Where(x => x.p_id == id).ToList();
            ViewBag.show_single_pro = Get_single_pro;
            return View();
        }
        List<CartDetails> c1 = new List<CartDetails>();
        [HttpPost]
        public ActionResult Product_info(tb_product tp,string qty,int id)
        {
            tb_product p = db.tb_product.Where(x => x.p_id == id).SingleOrDefault();
            var get = db.tb_product.ToList();

            CartDetails c = new CartDetails();
            c.PRODUCTID = p.p_id;
            c.PRODUCTNAME = p.product_name;
            c.PRODUCTIMAGE = p.product_image;
            c.PRODUCTPRICE = Convert.ToInt32(p.product_price);
            c.PROQTY = Convert.ToInt32(qty);
            c.BILL = c.PRODUCTPRICE * c.PROQTY;
            if(Session["cart"]==null)
            {
                c1.Add(c);
                Session["cart"] = c1;
            }
            else
            {
                List<CartDetails> c2 = Session["cart"] as List<CartDetails>;
                int flag = 0;
                foreach (var item in c2)
                {
                    if(item.PRODUCTID==c.PRODUCTID)
                    {
                        item.PROQTY += c.PROQTY;
                        item.BILL += c.BILL;
                        flag = 1;
                    }
                }
                if(flag==0)
                {
                    c2.Add(c);
                }
            }

            return RedirectToAction("Index","Home");
        }

        public ActionResult CheckOut()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckOut(tb_order to)
        {
            if(Session["uid"]!=null)
            {
                List<CartDetails> c1 = Session["cart"] as List<CartDetails>;
                tb_invoice i = new tb_invoice();
                i.userID = Convert.ToInt32(Session["uid"].ToString());
                i.invoice_date = DateTime.Now;
                i.total_bill = Convert.ToInt32(Session["total"]);
                i.entrydate = DateTime.Now;
                db.tb_invoice.Add(i);
                db.SaveChanges();

                foreach (var item in c1)
                {
                    tb_order o = new tb_order();
                    o.product_id = item.PRODUCTID;
                    o.invoice_id = i.invoice_id;
                    o.userID = Convert.ToInt32(Session["uid"].ToString());
                    o.o_date = DateTime.Now;
                    o.o_unitPrice = Convert.ToInt32(item.PRODUCTPRICE);
                    o.qty = item.PROQTY;
                    o.bill = Convert.ToInt32(item.BILL);
                    o.entry_date = DateTime.Now;
                    db.tb_order.Add(o);
                    db.SaveChanges();
                }
                return RedirectToAction("Shipment_Information","Carts");
            }
            else
            {
               return RedirectToAction("Customer_Login", "User");
            }
        }

        public ActionResult Shipment_Information()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Shipment_Information(tb_shipment ts)
        {
            tb_shipment s = new tb_shipment();
            s.userID = Convert.ToInt32(Session["uid"].ToString());
            s.fullName = Request.Form["fullName"];
            s.phone_number = Request.Form["phone_number"];
            s.shipment_address = Request.Form["shipment_address"];
            s.email = Request.Form["email"];
            s.card_number = Request.Form["card_number"];
            s.card_expiration = Request.Form["card_expiration"];
            s.total_balance = Convert.ToInt32(Session["total"]);
            s.entry_date = DateTime.Now;
            db.tb_shipment.Add(s);
            db.SaveChanges();
            Session["cart"] = null;
            Session["total"] = null;
            Session["qty"] = null;
            return RedirectToAction("CartEnd", "Carts");
        }

        public ActionResult CartEnd()
        {
            return View();
        }
    }
}