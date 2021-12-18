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
    public class GradeController : Controller
    {
        // GET: Grade
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
                var gradesData = (from grade in db.Grades

                                       select new GradeViewModel
                                       {
                                           id = grade.id,
                                           name = grade.name,
                                           created_at = grade.created_at,
                                       })
                                       .AsEnumerable()
                                       .Select(s=>new GradeViewModel {
                                           id = s.id,
                                           name = s.name,
                                           created_at = s.created_at,
                                           string_created_at = ((DateTime)s.created_at).ToString("yyyy-MM-dd")
                                       });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    gradesData = gradesData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = gradesData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = gradesData.Count();

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
        public JsonResult saveGrade(GradeViewModel gradeVM)
        {
            if (gradeVM.id == 0)
            {
                Grade grade = AutoMapper.Mapper.Map<GradeViewModel, Grade>(gradeVM);

                grade.updated_at = DateTime.Now;
                grade.created_at = DateTime.Now;

                db.Grades.Add(grade);
            }
            else
            {
                Grade oldGrade = db.Grades.Find(gradeVM.id);

                oldGrade.name = gradeVM.name;
                oldGrade.updated_at = DateTime.Now;

                db.Entry(oldGrade).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteGrade(int id)
        {
            Grade deleteGrade = db.Grades.Find(id);
            db.Grades.Remove(deleteGrade);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }
    }
}