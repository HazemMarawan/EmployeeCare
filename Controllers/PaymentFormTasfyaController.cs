using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using EmployeeCare.Auth;
using EmployeeCare.Enum;
using EmployeeCare.Models;
using EmployeeCare.ViewModel;
namespace EmployeeCare.Controllers
{
    [CustomAuthenticationFilter]
    public class PaymentFormTasfyaController : Controller
    {
        EmployeeCareDbContext db = new EmployeeCareDbContext();
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
                var paymentFormData = (from paymentform in db.PaymentForms
                                       join employeeDocument in db.EmployeeDocuments on paymentform.employee_document_id equals employeeDocument.id
                                       join employee in db.Employees on employeeDocument.employee_id equals employee.id
                                       join document in db.Documents on employeeDocument.document_id equals document.id
                                       join decision in db.Decisions on paymentform.decision_id equals decision.id
                                       select new PaymentFormViewModel
                                       {
                                           id = paymentform.id,
                                           employee_name = employee.name,
                                           document_name = document.name,
                                           created_at = paymentform.created_at,
                                           decision_name = decision.title,
                                           employee_document_id = paymentform.employee_document_id,
                                           employee_id = employeeDocument.employee_id,
                                           decision_id = paymentform.decision_id,
                                           salary = paymentform.salary,
                                           no_of_months = paymentform.no_of_months,
                                           last_paid_installment = paymentform.last_paid_installment,
                                           deduct_amount_from_takaful = paymentform.deduct_amount_from_takaful,
                                           installment_need_deduct = paymentform.installment_need_deduct,
                                           debt_need_deduct = paymentform.debt_need_deduct,
                                           membership_subscription_deduct = paymentform.membership_subscription_deduct,
                                           final_paid = paymentform.final_paid,
                                           notes = paymentform.notes,
                                           type = paymentform.type,
                                           approval_status = paymentform.approval_status
                                       }).AsEnumerable().Select(s => new PaymentFormViewModel
                                       {
                                           id = s.id,
                                           employee_name = s.employee_name,
                                           document_name = s.document_name,
                                           created_at = s.created_at,
                                           decision_name = s.decision_name,
                                           employee_document_id = s.employee_document_id,
                                           employee_id = s.employee_id,
                                           decision_id = s.decision_id,
                                           salary = s.salary,
                                           no_of_months = s.no_of_months,
                                           last_paid_installment = s.last_paid_installment,
                                           deduct_amount_from_takaful = s.deduct_amount_from_takaful,
                                           installment_need_deduct = s.installment_need_deduct,
                                           debt_need_deduct = s.debt_need_deduct,
                                           membership_subscription_deduct = s.membership_subscription_deduct,
                                           final_paid = s.final_paid,
                                           notes = s.notes,
                                           type = s.type,
                                           approval_status = s.approval_status,
                                           string_created_at = ((DateTime)s.created_at).ToString("yyyy-MM-dd")
                                       }).Where(s => s.approval_status == 1 && s.type == (int)PaymentFormTypes.Tasfya);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    paymentFormData = paymentFormData.Where(m => m.notes.ToLower().Contains(searchValue.ToLower())
                        || m.id.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.employee_name.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.document_name.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.decision_name.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.salary.ToString().ToLower().Contains(searchValue.ToLower())
                       );
                }

                //total number of rows count     
                var displayResult = paymentFormData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = paymentFormData.Count();

                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = displayResult

                }, JsonRequestBehavior.AllowGet);

            }
            ViewBag.employees = db.Employees.Select(d => new { d.id, d.name }).ToList();
            ViewBag.decisions = db.Decisions.Select(d => new { d.id, d.title }).ToList();

            return View();
        }
        [HttpPost]
        public JsonResult savePaymentForm(PaymentFormViewModel paymentFormVM)
        {
            if (paymentFormVM.id == 0)
            {
                PaymentForm paymentForm = AutoMapper.Mapper.Map<PaymentFormViewModel, PaymentForm>(paymentFormVM);

                paymentForm.updated_at = DateTime.Now;
                paymentForm.created_at = DateTime.Now;

                db.PaymentForms.Add(paymentForm);
            }
            else
            {
                PaymentForm oldPaymentForm = db.PaymentForms.Find(paymentFormVM.id);

                oldPaymentForm.employee_document_id = paymentFormVM.employee_document_id;
                oldPaymentForm.decision_id = paymentFormVM.decision_id;
                oldPaymentForm.salary = paymentFormVM.salary;
                oldPaymentForm.no_of_months = paymentFormVM.no_of_months;
                oldPaymentForm.last_paid_installment = paymentFormVM.last_paid_installment;
                oldPaymentForm.deduct_amount_from_takaful = paymentFormVM.deduct_amount_from_takaful;
                oldPaymentForm.installment_need_deduct = paymentFormVM.installment_need_deduct;
                oldPaymentForm.debt_need_deduct = paymentFormVM.debt_need_deduct;
                oldPaymentForm.membership_subscription_deduct = paymentFormVM.membership_subscription_deduct;
                oldPaymentForm.final_paid = paymentFormVM.final_paid;
                oldPaymentForm.notes = paymentFormVM.notes;
                oldPaymentForm.approval_status = paymentFormVM.approval_status;
                oldPaymentForm.managerial_fees = paymentFormVM.managerial_fees;
                oldPaymentForm.installments = paymentFormVM.installments;
                oldPaymentForm.cheque_cost = paymentFormVM.cheque_cost;
                oldPaymentForm.other_income = paymentFormVM.other_income;
                oldPaymentForm.total_deduction = paymentFormVM.total_deduction;
                oldPaymentForm.cheque_number = paymentFormVM.cheque_number;
                oldPaymentForm.updated_at = DateTime.Now;

                db.Entry(oldPaymentForm).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deletePaymentForm(int id)
        {
            PaymentForm deletePaymentForm = db.PaymentForms.Find(id);
            db.PaymentForms.Remove(deletePaymentForm);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult View(int id)
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "PaymentFormPart1.rpt"));
            List<Setting> settings = db.Settings.ToList();
            List<PaymentFormReport> paymentFormViewModel = (from paymentform in db.PaymentForms
                                                            join employeeDocument in db.EmployeeDocuments on paymentform.employee_document_id equals employeeDocument.id
                                                            join employee in db.Employees on employeeDocument.employee_id equals employee.id
                                                            join destination in db.Destinations on employee.destination_id equals destination.id
                                                            join grade in db.Grades on employee.grade_id equals grade.id
                                                            join document in db.Documents on employeeDocument.document_id equals document.id
                                                            join decision in db.Decisions on paymentform.decision_id equals decision.id
                                                            select new PaymentFormReport
                                                            {
                                                                id = paymentform.id,
                                                                employee_name = employee.name,
                                                                document_name = document.name,
                                                                grade_name = grade.name,
                                                                destinaion_name = destination.name,
                                                                created_at = (DateTime)paymentform.created_at,
                                                                decision_name = decision.title,
                                                                employee_document_id = (int)paymentform.employee_document_id,
                                                                employee_id = (int)employeeDocument.employee_id,
                                                                decision_id = (int)paymentform.decision_id,
                                                                salary = (double)paymentform.salary,
                                                                no_of_months = (double)paymentform.no_of_months,
                                                                last_paid_installment = paymentform.last_paid_installment,
                                                                deduct_amount_from_takaful = (double)paymentform.deduct_amount_from_takaful,
                                                                installment_need_deduct = (double)paymentform.installment_need_deduct,
                                                                debt_need_deduct = (double)paymentform.debt_need_deduct,
                                                                membership_subscription_deduct = (double)paymentform.membership_subscription_deduct,
                                                                final_paid = (double)paymentform.final_paid,
                                                                notes = paymentform.notes,
                                                                approval_status = (int)paymentform.approval_status,
                                                                managerial_fees = (double)paymentform.managerial_fees,
                                                                installments = (int)paymentform.installments,
                                                                cheque_cost = (double)paymentform.cheque_cost,
                                                                other_income = (double)paymentform.other_income,
                                                                total_deduction = (double)paymentform.total_deduction,
                                                                cheque_number = paymentform.cheque_number,
                                                                modir_3am_elgam3ia = db.Settings.Where(set => set.name == "modir_3am_elgam3ia").FirstOrDefault().value,
                                                                modir_elhesabat = db.Settings.Where(set => set.name == "modir_elhesabat").FirstOrDefault().value,
                                                                amin_elsandok = db.Settings.Where(set => set.name == "amin_elsandok").FirstOrDefault().value,
                                                                modir_edart_elwasika = db.Settings.Where(set => set.name == "modir_edart_elwasika").FirstOrDefault().value,
                                                                national_id = employee.national_id,
                                                                string_birth_date = ((DateTime)employee.birth_date).ToString()

                                                            }).Where(s => s.id == id).ToList();

            rd.SetDataSource(paymentFormViewModel);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            rd.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
            rd.PrintOptions.ApplyPageMargins(new CrystalDecisions.Shared.PageMargins(5, 5, 5, 5));
            rd.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5;

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/pdf", "PaymentForm" + paymentFormViewModel[0].employee_name + DateTime.Now.ToString() + ".pdf");
        }
    }
}