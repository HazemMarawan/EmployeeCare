using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using EmployeeCare.Auth;
using EmployeeCare.Models;
using EmployeeCare.ViewModel;

namespace EmployeeCare.Controllers
{
    [CustomAuthenticationFilter]
    public class AccountantController : Controller
    {
        EmployeeCareDbContext db = new EmployeeCareDbContext();

        // GET: Accountant
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
                                           approval_status = paymentform.approval_status,
                                           managerial_fees = paymentform.managerial_fees == null? db.Deductions.Select(m => m.managerial_fees).FirstOrDefault().Value: paymentform.managerial_fees,
                                           installments = paymentform.installments == null ? db.Deductions.Select(m => m.installments).FirstOrDefault().Value: paymentform.installments,
                                           subscription = paymentform.subscription == null ? db.Deductions.Select(m => m.subscription).FirstOrDefault().Value: paymentform.subscription,
                                           other_income = paymentform.other_income == null ? db.Deductions.Select(m => m.other_income).FirstOrDefault().Value : paymentform.other_income,
                                           cheque_cost = paymentform.cheque_cost == null ? (int)db.Deductions.Select(m => m.cheque_cost).FirstOrDefault().Value : (int)paymentform.cheque_cost,
                                           total_deduction = (double)db.Deductions.Select(m => m.total_deduction).FirstOrDefault().Value,
                                           cheque_number = paymentform.cheque_number
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
                                           approval_status = s.approval_status,
                                           string_created_at = ((DateTime)s.created_at).ToString("yyyy-MM-dd"),
                                           managerial_fees = s.managerial_fees,
                                           installments = s.installments,
                                           other_income = s.other_income,
                                           cheque_cost = s.cheque_cost,
                                           total_deduction = s.total_deduction,
                                           cheque_number = s.cheque_number,
                                           subscription = s.subscription
                                       }).Where(s => s.approval_status == 2);

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
        public JsonResult saveAccountant(PaymentFormViewModel paymentFormVM)
        {
         
            PaymentForm oldPaymentForm = db.PaymentForms.Find(paymentFormVM.id);

            oldPaymentForm.approval_status = paymentFormVM.approval_status;
            oldPaymentForm.managerial_fees = paymentFormVM.managerial_fees;
            oldPaymentForm.installments = paymentFormVM.installments;
            oldPaymentForm.subscription = paymentFormVM.subscription;
            oldPaymentForm.cheque_cost = paymentFormVM.cheque_cost;
            oldPaymentForm.other_income = paymentFormVM.other_income;
            oldPaymentForm.total_deduction = paymentFormVM.managerial_fees + paymentFormVM.installments + paymentFormVM.cheque_cost + paymentFormVM.other_income;
            oldPaymentForm.cheque_number = paymentFormVM.cheque_number;
            oldPaymentForm.updated_at = DateTime.Now;

            db.Entry(oldPaymentForm).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult View(int id)
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "PaymentFormFinal.rpt"));
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
                                                                gneh_managerial_fees = (int)paymentform.managerial_fees,
                                                                ersh_managerial_fees = (double)((paymentform.managerial_fees - (int)paymentform.managerial_fees)*100),
                                                                installments = (int)paymentform.installments,
                                                                gneh_installments = (int)paymentform.installments,
                                                                ersh_installments = (double)((paymentform.installments - (int)paymentform.installments) * 100),
                                                                cheque_cost = (double)paymentform.cheque_cost,
                                                                gneh_cheque_cost = (int)paymentform.cheque_cost,
                                                                ersh_cheque_cost = (double)((paymentform.cheque_cost - (int)paymentform.cheque_cost) * 100),
                                                                other_income = (double)paymentform.other_income,
                                                                gneh_other_income = (int)paymentform.other_income,
                                                                ersh_other_income = (double)((paymentform.other_income - (int)paymentform.other_income) * 100),
                                                                total_deduction = (double)paymentform.total_deduction,
                                                                gneh_total_deduction = (int)paymentform.total_deduction,
                                                                ersh_total_deduction = (double)((paymentform.total_deduction - (int)paymentform.total_deduction) * 100),
                                                                subscription = (double)paymentform.subscription,
                                                                gneh_subscription = (int)paymentform.subscription,
                                                                ersh_subscription = (double)((paymentform.subscription - (int)paymentform.subscription) * 100),
                                                                cheque_number = paymentform.cheque_number,
                                                                modir_3am_elgam3ia = db.Settings.Where(set => set.name == "modir_3am_elgam3ia").FirstOrDefault().value,
                                                                modir_elhesabat = db.Settings.Where(set => set.name == "modir_elhesabat").FirstOrDefault().value,
                                                                amin_elsandok = db.Settings.Where(set => set.name == "amin_elsandok").FirstOrDefault().value,
                                                                modir_edart_elwasika = db.Settings.Where(set => set.name == "modir_edart_elwasika").FirstOrDefault().value,
                                                                national_id = employee.national_id,
                                                                string_birth_date = ((DateTime)employee.birth_date).ToString(),
                                                                final_paid_after_deduction = (double)(paymentform.final_paid - paymentform.total_deduction)

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