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
    public class PaidOffController : Controller
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
                                           managerial_fees = paymentform.managerial_fees == null ? db.Deductions.Select(m => m.managerial_fees).FirstOrDefault().Value : paymentform.managerial_fees,
                                           installments = paymentform.installments == null ? db.Deductions.Select(m => m.installments).FirstOrDefault().Value : paymentform.installments,
                                           other_income = paymentform.other_income == null ? db.Deductions.Select(m => m.other_income).FirstOrDefault().Value : paymentform.other_income,
                                           cheque_cost = paymentform.cheque_cost == null ? (int)db.Deductions.Select(m => m.cheque_cost).FirstOrDefault().Value : (int)paymentform.cheque_cost,
                                           total_deduction = (double)db.Deductions.Select(m => m.total_deduction).FirstOrDefault().Value,

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
                                           managerial_fees = db.Deductions.Select(m => m.managerial_fees).FirstOrDefault().Value,
                                           installments = db.Deductions.Select(m => m.installments).FirstOrDefault().Value,
                                           other_income = db.Deductions.Select(m => m.other_income).FirstOrDefault().Value,
                                           cheque_cost = (int)db.Deductions.Select(m => m.cheque_cost).FirstOrDefault().Value,
                                           total_deduction = (double)db.Deductions.Select(m => m.total_deduction).FirstOrDefault().Value,

                                       }).Where(s => s.approval_status == 3);

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

    }
}