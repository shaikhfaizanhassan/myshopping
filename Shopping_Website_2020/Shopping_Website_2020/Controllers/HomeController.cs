using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopping_Website_2020.Models;
using System.Data.SqlClient;

namespace Shopping_Website_2020.Controllers
{
    public class HomeController : Controller
    {
        shopping_website_DBEntities db = new shopping_website_DBEntities();
        // GET: Home
        public ActionResult Index()
        {
            var get_Pro = db.tb_product.ToList();
            ViewBag.Pro_show = get_Pro;
            if(Session["cart"]!=null)
            {
                float x = 0;
                int q = 0;
                List<CartDetails> c3 = Session["cart"] as List<CartDetails>;
                foreach (var item in c3)
                {
                    x += item.BILL;
                    q += item.PROQTY;
                }
                Session["total"] = x;
                Session["qty"] = q;
            }
            return View();
        }

        public ActionResult CategoryPage(int id)
        {
            var get_C_pro = db.tb_product.Where(x => x.category_id == id).ToList();
            ViewBag.show_Cat_pro = get_C_pro;

            SqlCommand cmd = new SqlCommand("select * from tb_product where category_id");

            return View();
        }

        public ActionResult BrandPage(int id)
        {
            var get_B_pro = db.tb_product.Where(x => x.brand_id == id).ToList();
            ViewBag.show_brand_product = get_B_pro;
            return View();
        }

        public ActionResult SearchProduct(string Searching)
        {
            var s = db.tb_product.Where(x => x.product_name.Contains(Searching) || Searching == null).ToList();
            ViewBag.p = s;
            return View(s);
        }
    }
}