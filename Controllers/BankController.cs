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
    public class BankController : Controller
    {
        EmployeeCareDbContext db = new EmployeeCareDbContext();
        // GET: Bank
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
                var banknData = (from bank in db.Banks
                                       select new BankViewModel
                                       {
                                           id = bank.id,
                                           name = bank.name,
                                           created_at = bank.created_at
                                       }).AsEnumerable().Select(s => new BankViewModel
                                       {
                                           id = s.id,
                                           name = s.name,
                                           string_created_at = ((DateTime)s.created_at).ToString("yyyy-MM-dd")
                                       });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    banknData = banknData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = banknData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = banknData.Count();

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
        public JsonResult saveBank(BankViewModel bankVM)
        {
            if (bankVM.id == 0)
            {
                Bank bank = AutoMapper.Mapper.Map<BankViewModel, Bank>(bankVM);

                bank.updated_at = DateTime.Now;
                bank.created_at = DateTime.Now;

                db.Banks.Add(bank);
            }
            else
            {
                Bank oldBank = db.Banks.Find(bankVM.id);

                oldBank.name = bankVM.name;
                oldBank.updated_at = DateTime.Now;

                db.Entry(oldBank).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteBank(int id)
        {
            Bank deleteBank = db.Banks.Find(id);
            db.Banks.Remove(deleteBank);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }
    }
}