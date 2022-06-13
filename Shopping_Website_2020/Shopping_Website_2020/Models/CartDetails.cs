using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping_Website_2020.Models
{
    public class CartDetails
    {
        public int PRODUCTID { get; set; }
        public string PRODUCTNAME { get; set; }
        public float PRODUCTPRICE { get; set; }
        public string PRODUCTIMAGE { get; set; }
        public string PRODUCTDESCRIBTION { get; set; }
        public int PROQTY { get; set; }
        public float BILL { get; set; }
    }
}