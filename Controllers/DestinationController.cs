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
    public class DestinationController : Controller
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
                var destinationData = (from destination in db.Destinations

                                      select new DestinationViewModel
                                      {
                                          id = destination.id,
                                          name = destination.name,
                                          created_at = destination.created_at
                                      }).AsEnumerable().Select(s=>new DestinationViewModel {
                                          id = s.id,
                                          name = s.name,
                                          string_created_at = ((DateTime)s.created_at).ToString("yyyy-MM-dd")
                                      });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    destinationData = destinationData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = destinationData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = destinationData.Count();

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
        public JsonResult saveDestination(DestinationViewModel destinationVM)
        {
            if (destinationVM.id == 0)
            {
                Destination destination = AutoMapper.Mapper.Map<DestinationViewModel, Destination>(destinationVM);

                //destination.created_by = (int)Session["id"];
                destination.updated_at = DateTime.Now;
                destination.created_at = DateTime.Now;

                db.Destinations.Add(destination);
            }
            else
            {
                Destination oldDestination = db.Destinations.Find(destinationVM.id);

                oldDestination.name = destinationVM.name;
                oldDestination.updated_at = DateTime.Now;

                db.Entry(oldDestination).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteDestination(int id)
        {
            Destination deleteDestination = db.Destinations.Find(id);
            db.Destinations.Remove(deleteDestination);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }
    }
}