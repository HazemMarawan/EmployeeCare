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
    public class a3da2LeganController : Controller
    {
        // GET: a3da2Legan
        EmployeeCareDbContext db = new EmployeeCareDbContext();

        public ActionResult Index(int id)
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
                var A3Da2LegansData = (from a3da2Legan in db.A3Da2Legans
                                         join employee in db.Employees on a3da2Legan.employee_id equals employee.id
                                         join bank in db.Banks on a3da2Legan.bank_id equals bank.id

                                         select new a3da2LeganViewModel
                                         {
                                             id = a3da2Legan.id,
                                             employee_id = a3da2Legan.employee_id,
                                             status = a3da2Legan.status,
                                             cheque_number = a3da2Legan.cheque_number,
                                             date = a3da2Legan.date,
                                             date_submitted = a3da2Legan.date_submitted,
                                             legan_mosa3dat_id = a3da2Legan.legan_mosa3dat_id,
                                             bank_id = a3da2Legan.bank_id,
                                             created_at = a3da2Legan.created_at,
                                             bank_name = bank.name,
                                             employee_name = employee.name
                                         }).Where(l => l.legan_mosa3dat_id == id).AsEnumerable().Select(s => new a3da2LeganViewModel
                                         {
                                             id = s.id,
                                             date = s.date,
                                             status = s.status,
                                             employee_id = s.employee_id,
                                             bank_id = s.bank_id,
                                             cheque_number = s.cheque_number,
                                             bank_name = s.bank_name,
                                             employee_name = s.employee_name,
                                             legan_mosa3dat_id = s.legan_mosa3dat_id,
                                             date_submitted = s.date_submitted,
                                             string_date = ((DateTime)s.date).ToString("yyyy-MM-dd"),
                                             string_date_submitted = ((DateTime)s.date_submitted).ToString("yyyy-MM-dd")
                                         });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    A3Da2LegansData = A3Da2LegansData.Where(m => m.employee_name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = A3Da2LegansData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = A3Da2LegansData.Count();

                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = displayResult

                }, JsonRequestBehavior.AllowGet);

            }

            ViewBag.legan_mosa3dat_id = id;
            ViewBag.employees = db.Employees.Select(d => new { d.id, d.name }).ToList();
            ViewBag.banks = db.Banks.Select(d => new { d.id, d.name }).ToList();

            return View();
        }
        [HttpPost]
        public JsonResult saveA3Da2Legans(a3da2LeganViewModel A3Da2LegansVM)
        {
            if (A3Da2LegansVM.id == 0)
            {
                a3da2Legan A3Da2Legans = AutoMapper.Mapper.Map<a3da2LeganViewModel, a3da2Legan>(A3Da2LegansVM);

                //A3Da2Legans.created_by = (int)Session["id"];
                A3Da2Legans.updated_at = DateTime.Now;
                A3Da2Legans.created_at = DateTime.Now;

                db.A3Da2Legans.Add(A3Da2Legans);
            }
            else
            {
                a3da2Legan oldA3Da2Legans = db.A3Da2Legans.Find(A3Da2LegansVM.id);

                oldA3Da2Legans.employee_id = A3Da2LegansVM.employee_id;
                oldA3Da2Legans.date = A3Da2LegansVM.date;
                oldA3Da2Legans.date_submitted = A3Da2LegansVM.date_submitted;
                oldA3Da2Legans.status = A3Da2LegansVM.status;
                oldA3Da2Legans.cheque_number = A3Da2LegansVM.cheque_number;
                oldA3Da2Legans.legan_mosa3dat_id = A3Da2LegansVM.legan_mosa3dat_id;
                oldA3Da2Legans.updated_at = DateTime.Now;

                db.Entry(oldA3Da2Legans).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteA3Da2Legans(int id)
        {
            a3da2Legan deleteA3Da2Legans = db.A3Da2Legans.Find(id);
            db.A3Da2Legans.Remove(deleteA3Da2Legans);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}