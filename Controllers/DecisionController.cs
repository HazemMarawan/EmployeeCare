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
    public class DecisionController : Controller
    {
        EmployeeCareDbContext db = new EmployeeCareDbContext();
        // GET: Decision
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
                var decisionData = (from decision in db.Decisions
                                       join destination in db.Destinations on decision.destination_id equals destination.id
                                       select new DecisionViewModel
                                       {
                                           id = decision.id,
                                           destination_id = decision.destination_id,
                                           destination_name = destination.name,
                                           title = decision.title,
                                           decision_date = decision.decision_date,
                                           decision_type = decision.decision_type,
                                           notes = decision.notes,
                                           created_at = destination.created_at,
                                           decision_documents = db.DecisionDocuments.Where(s=>s.decision_id == decision.id).Select(s=>s.path).ToList()
                                       }).AsEnumerable().Select(s => new DecisionViewModel
                                       {
                                           id = s.id,
                                           destination_id = s.destination_id,
                                           destination_name = s.destination_name,
                                           title = s.title,
                                           decision_date = s.decision_date,
                                           decision_type = s.decision_type,
                                           notes = s.notes,
                                           created_at = s.created_at,
                                           decision_documents = s.decision_documents,
                                           string_created_at = s.created_at != null ? ((DateTime)s.created_at).ToString("yyyy-MM-dd") : null,
                                           string_decision_date = s.decision_date != null ?((DateTime)s.decision_date).ToString("yyyy-MM-dd") : null
                                       });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    decisionData = decisionData.Where(m => m.title.ToLower().Contains(searchValue.ToLower()) 
                    || m.id.ToString().ToLower().Contains(searchValue.ToLower())
                    || m.destination_name.ToString().ToLower().Contains(searchValue.ToLower())
                    );
                }

                //total number of rows count     
                var displayResult = decisionData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = decisionData.Count();

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
        public JsonResult saveDecision(DecisionViewModel decisionVM)
        {
            if (decisionVM.id == 0)
            {
                Decision decision = AutoMapper.Mapper.Map<DecisionViewModel, Decision>(decisionVM);

                decision.updated_at = DateTime.Now;
                decision.created_at = DateTime.Now;

                db.Decisions.Add(decision);

                if(decisionVM.files != null)
                {
                    foreach (var file in decisionVM.files)
                    {
                        DecisionDocument decisionDocument = new DecisionDocument();
                        Guid guid = Guid.NewGuid();
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Uploads/Decision/Documents/") + guid.ToString() + "_Decision_Document" + Path.GetExtension(file.FileName));
                        file.SaveAs(ServerSavePath);
                        decisionDocument.path = "/Uploads/Decision/Documents/" + guid.ToString() + "_Decision_Document" + Path.GetExtension(file.FileName);
                        decisionDocument.created_at = DateTime.Now;
                        decisionDocument.decision_id = decision.id;

                        db.DecisionDocuments.Add(decisionDocument);
                    }
                    db.SaveChanges();
                }
            }
            else
            {
                Decision oldDecision = db.Decisions.Find(decisionVM.id);

                oldDecision.title = decisionVM.title;
                oldDecision.destination_id = decisionVM.destination_id;
                oldDecision.decision_date = decisionVM.decision_date;
                oldDecision.decision_type = decisionVM.decision_type;
                oldDecision.notes = decisionVM.notes;
                oldDecision.updated_at = DateTime.Now;

                db.Entry(oldDecision).State = System.Data.Entity.EntityState.Modified;

                db.DecisionDocuments.Where(s => s.decision_id == oldDecision.id).ToList().ForEach(s => db.DecisionDocuments.Remove(s));

                if (decisionVM.files != null)
                {
                    foreach (var file in decisionVM.files)
                    {
                        DecisionDocument decisionDocument = new DecisionDocument();
                        Guid guid = Guid.NewGuid();
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Uploads/Decision/Documents/") + guid.ToString() + "_Decision_Document" + Path.GetExtension(file.FileName));
                        file.SaveAs(ServerSavePath);
                        decisionDocument.path = "/Uploads/Decision/Documents/" + guid.ToString() + "_Decision_Document" + Path.GetExtension(file.FileName);
                        decisionDocument.created_at = DateTime.Now;
                        decisionDocument.decision_id = oldDecision.id;

                        db.DecisionDocuments.Add(decisionDocument);
                    }
                    db.SaveChanges();
                }
            }
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteDecision(int id)
        {
            db.DecisionDocuments.Where(s => s.decision_id == id).ToList().ForEach(s => db.DecisionDocuments.Remove(s));

            Decision deleteDecision = db.Decisions.Find(id);
            db.Decisions.Remove(deleteDecision);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult saveDecisionFiles(DecisionViewModel decisionVM)
        {

            if (decisionVM.files != null)
            {
                foreach (var file in decisionVM.files)
                {
                    DecisionDocument decisionDocument = new DecisionDocument();
                    Guid guid = Guid.NewGuid();
                    var InputFileName = Path.GetFileName(file.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Uploads/Decision/Documents/") + guid.ToString() + "_Decision_Document" + Path.GetExtension(file.FileName));
                    file.SaveAs(ServerSavePath);
                    decisionDocument.path = "/Uploads/Decision/Documents/" + guid.ToString() + "_Decision_Document" + Path.GetExtension(file.FileName);
                    decisionDocument.created_at = DateTime.Now;
                    decisionDocument.decision_id = decisionVM.id;

                    db.DecisionDocuments.Add(decisionDocument);
                }
                db.SaveChanges();

            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }
    }
}