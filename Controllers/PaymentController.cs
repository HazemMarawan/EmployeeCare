using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeCare.Auth;
using EmployeeCare.Models;
using EmployeeCare.ViewModel;
namespace EmployeeCare.Controllers
{
    [CustomAuthenticationFilter]
    public class PaymentController : Controller
    {
        EmployeeCareDbContext db = new EmployeeCareDbContext();
        // GET: Destination
        public ActionResult Index()
        {

            if (Request.IsAjaxRequest())
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                var from_date = Request.Form.GetValues("columns[0][search][value]")[0];
                var to_date = Request.Form.GetValues("columns[1][search][value]")[0];
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                // Getting all data    
                var paymentData = (from payment in db.PaymentTypes

                                       select new PaymentTypeViewModel
                                       {
                                           id = payment.id,
                                           name = payment.name,
                                           created_at = payment.created_at
                                       }).AsEnumerable().Select(s => new PaymentTypeViewModel
                                       {
                                           id = s.id,
                                           name = s.name,
                                           string_created_at = ((DateTime)s.created_at).ToString("yyyy-MM-dd")
                                       });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    paymentData = paymentData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = paymentData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = paymentData.Count();

                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = displayResult

                }, JsonRequestBehavior.AllowGet);

            }
            return View();
        }
        [HttpPost]
        public JsonResult savePayment(PaymentTypeViewModel paymentVM)
        {
            if (paymentVM.id == 0)
            {
                PaymentType payment = AutoMapper.Mapper.Map<PaymentTypeViewModel, PaymentType>(paymentVM);

                payment.updated_at = DateTime.Now;
                payment.created_at = DateTime.Now;

                db.PaymentTypes.Add(payment);
            }
            else
            {
                PaymentType oldPayment = db.PaymentTypes.Find(paymentVM.id);

                oldPayment.name = paymentVM.name;
                oldPayment.updated_at = DateTime.Now;

                db.Entry(oldPayment).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deletePayment(int id)
        {
            PaymentType deletePayment = db.PaymentTypes.Find(id);
            db.PaymentTypes.Remove(deletePayment);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }
    }
}