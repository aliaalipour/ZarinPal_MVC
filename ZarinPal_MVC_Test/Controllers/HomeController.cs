using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZarinPal_MVC_Test.Models;

namespace ZarinPal_MVC_Test.Controllers
{
    public class HomeController : Controller
    {

        DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {

            return View(db.Products.ToList());
        }

        // ارسال درخواست به در درگاه پرداخت
        public ActionResult FinalPayment(int id)
        {
            //پیدا کردن شماره محصول
            Product product = db.Products.Find(id);

            //ذخیره محصول در جدول سفارشات
            Order oOrder = new Order();
            oOrder.DateTime = DateTime.Now;
            oOrder.IsFinaly = false;
            oOrder.Sum = product.Price;

            db.Orders.Add(oOrder);
            db.SaveChanges();


            //اتصال به درگاه زرین پال
            System.Net.ServicePointManager.Expect100Continue = false;
            ZarinPalTest.PaymentGatewayImplementationServicePortTypeClient zp =
                new ZarinPalTest.PaymentGatewayImplementationServicePortTypeClient();

            string Authority;

            int Status =
                zp.PaymentRequest("YOUR-ZARINPAL-MERCHANT-CODE", oOrder.Sum, 
                " درگاه تست زرین پال", "aliaalipour_official@gmail.com", "09169870903", "http://localhost:14805/Home/Verify/" + oOrder.OrderID, out Authority);

            //100 == Ok
            if (Status == 100)
            {
                //When Publish
                //Response.Redirect("https://www.zarinpal.com/pg/StartPay/" + Authority);

                //When Test
                Response.Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + Authority);
            }
            else
            {
                Response.Write("error: " + Status);
            }

            //برای نمایش خطا به علت عدم ارسال به درگاه
            return View();
        }



        public ActionResult Verify(int id)
        {
            //پیدا کردن sum
            var order = db.Orders.Find(id);

            //Status == Ok || NOK
            //Authority == كد يكتاي شناسه مرجع درخواست
            if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null 
                && Request.QueryString["Authority"] != "" && Request.QueryString["Authority"] != null)
            {
                //اگر وضعیت پرداخت اوکی بود
                if (Request.QueryString["Status"].ToString().Equals("OK"))
                {
                    //jame tamame maghadir onja save hastesh
                    int Amount = order.Sum;
                    long RefID;
                    System.Net.ServicePointManager.Expect100Continue = false;
                    ZarinPalTest.PaymentGatewayImplementationServicePortTypeClient zp =
                        new ZarinPalTest.PaymentGatewayImplementationServicePortTypeClient();

                    int Status =
                        zp.PaymentVerification("YOUR-ZARINPAL-MERCHANT-CODE",
                        Request.QueryString["Authority"].ToString(), Amount, out RefID);

                    if (Status == 100)
                    {
                        ViewBag.IsSuccess = true;
                        ViewBag.RefId = RefID;

                        order.IsFinaly = true;
                        db.SaveChanges();
                        
                    }
                    else
                    {
                        ViewBag.Status = Status;
                    }

                }
                else
                {
                    Response.Write("Error! Authority: " + Request.QueryString["Authority"].ToString() + " Status: " + Request.QueryString["Status"].ToString());
                }
            }
            else
            {
                Response.Write("Invalid Input");
            }
            return View();
        }
    }
}