using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeCare.Auth;
using EmployeeCare.Models;
using EmployeeCare.ViewModel;

namespace EmployeeCare.Controllers
{
    [CustomAuthenticationFilter]
    public class OtherSystemController : Controller
    {
        EmployeeCareDbContext db = new EmployeeCareDbContext();

        // GET: OtherSystems
        public ActionResult Index()
        {
            User currentUser = Session["user"] as User;
           
            if (Request.IsAjaxRequest())
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                // Getting all data    
                var otherSystemData = (from otherSystem in db.OtherSystems
                                         select new OtherSystemViewModel
                                         {
                                             id = otherSystem.id,
                                             name = otherSystem.name,
                                             description = otherSystem.description,
                                             active = otherSystem.active,
                                             created_at = otherSystem.created_at,

                                         }).Where(n => n.active == 1);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    otherSystemData = otherSystemData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()));
                }

            
                //total number of rows count     
                var displayResult = otherSystemData.OrderByDescending(o => o.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = otherSystemData.Count();

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
        public JsonResult saveOtherSystem(OtherSystemViewModel OtherSystemViewModel)
        {

            if (OtherSystemViewModel.id == 0)
            {
                OtherSystem OtherSystem = AutoMapper.Mapper.Map<OtherSystemViewModel, OtherSystem>(OtherSystemViewModel);

                OtherSystem.created_at = DateTime.Now;
                //OtherSystem.created_by = Session["id"].ToString().ToInt();

                db.OtherSystems.Add(OtherSystem);
                db.SaveChanges();
            }
            else
            {

                OtherSystem oldOtherSystem = db.OtherSystems.Find(OtherSystemViewModel.id);

                oldOtherSystem.name = OtherSystemViewModel.name;
                oldOtherSystem.description = OtherSystemViewModel.description;
                oldOtherSystem.active = OtherSystemViewModel.active;
                //oldOtherSystem.updated_by = Session["id"].ToString().ToInt();
                oldOtherSystem.updated_at = DateTime.Now;

                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteOtherSystem(int id)
        {
            OtherSystem deleteOtherSystem = db.OtherSystems.Find(id);
            deleteOtherSystem.active = 0;
            //deleteOtherSystem.deleted_at = DateTime.Now;
            //deleteOtherSystem.deleted_by = Session["id"].ToString().ToInt();

            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}