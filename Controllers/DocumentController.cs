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
    public class DocumentController : Controller
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
                var documentsData = (from document in db.Documents
                                     join destination in db.Destinations on document.destination_id equals destination.id
                                     select new DocumentViewModel
                                     {
                                           id = document.id,
                                           name = document.name,
                                           description = document.description,
                                           destination_name = destination.name,
                                           destination_id = document.destination_id,
                                           created_at = destination.created_at
                                       }).AsEnumerable().Select(s => new DocumentViewModel
                                       {
                                           id = s.id,
                                           name = s.name,
                                           description = s.description,
                                           destination_name = s.destination_name,
                                           destination_id = s.destination_id,
                                           string_created_at = ((DateTime)s.created_at).ToString("yyyy-MM-dd")
                                       });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    documentsData = documentsData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) || m.destination_name.ToString().ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = documentsData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = documentsData.Count();

                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = displayResult

                }, JsonRequestBehavior.AllowGet);

            }
            ViewBag.destinations = db.Destinations.Select(d => new { d.id, d.name }).ToList();
            return View();
        }
        [HttpPost]
        public JsonResult saveDocument(DocumentViewModel documentVM)
        {
            if (documentVM.id == 0)
            {
                Document document = AutoMapper.Mapper.Map<DocumentViewModel, Document>(documentVM);

                document.updated_at = DateTime.Now;
                document.created_at = DateTime.Now;

                db.Documents.Add(document);
            }
            else
            {
                Document oldDocument = db.Documents.Find(documentVM.id);

                oldDocument.name = documentVM.name;
                oldDocument.description = documentVM.description;
                oldDocument.destination_id = documentVM.destination_id;
                oldDocument.updated_at = DateTime.Now;

                db.Entry(oldDocument).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteDocument(int id)
        {
            Document deleteDocument = db.Documents.Find(id);
            db.Documents.Remove(deleteDocument);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }
    }
}