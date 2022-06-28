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
    public class leganMosa3datController : Controller
    {
        EmployeeCareDbContext db = new EmployeeCareDbContext();

        // GET: leganMosa3dat
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
                var leganMosa3datData = (from leganMosa3dat in db.LeganMosa3Dats

                                       select new leganMosa3datViewModel
                                       {
                                           id = leganMosa3dat.id,
                                           name = leganMosa3dat.name,
                                           lagna_at = leganMosa3dat.lagna_at,
                                           created_at = leganMosa3dat.created_at
                                       }).AsEnumerable().Select(s => new leganMosa3datViewModel
                                       {
                                           id = s.id,
                                           name = s.name,
                                           lagna_at = s.lagna_at,
                                           string_created_at = ((DateTime)s.created_at).ToString("yyyy-MM-dd"),
                                           string_lagna_at = ((DateTime)s.lagna_at).ToString("yyyy-MM-dd")
                                       });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    leganMosa3datData = leganMosa3datData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = leganMosa3datData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = leganMosa3datData.Count();

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
        public JsonResult saveleganMosa3dat(leganMosa3datViewModel leganMosa3datVM)
        {
            if (leganMosa3datVM.id == 0)
            {
                leganMosa3dat leganMosa3dat = AutoMapper.Mapper.Map<leganMosa3datViewModel, leganMosa3dat>(leganMosa3datVM);

                //leganMosa3dat.created_by = (int)Session["id"];
                leganMosa3dat.updated_at = DateTime.Now;
                leganMosa3dat.created_at = DateTime.Now;

                db.LeganMosa3Dats.Add(leganMosa3dat);
            }
            else
            {
                leganMosa3dat oldleganMosa3dat = db.LeganMosa3Dats.Find(leganMosa3datVM.id);

                oldleganMosa3dat.name = leganMosa3datVM.name;
                oldleganMosa3dat.lagna_at = leganMosa3datVM.lagna_at;
                oldleganMosa3dat.updated_at = DateTime.Now;

                db.Entry(oldleganMosa3dat).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteleganMosa3dat(int id)
        {
            leganMosa3dat deleteleganMosa3dat = db.LeganMosa3Dats.Find(id);
            db.LeganMosa3Dats.Remove(deleteleganMosa3dat);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}