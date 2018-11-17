using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

     

namespace ZarinPal_MVC_Test.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int Sum { get; set; }
        public DateTime DateTime { get; set; }

        //عملیات پرداخت با موفقیت انجام شده؟
        public bool IsFinaly { get; set; }
    }
}