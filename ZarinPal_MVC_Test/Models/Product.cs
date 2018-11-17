using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ZarinPal_MVC_Test.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string ImageName { get; set; }

    }
}