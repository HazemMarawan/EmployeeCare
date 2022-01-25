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
    public class InvoiceController : Controller
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
                var invoicenData = (from invoice in db.Invoices
                                    join payment_type in db.PaymentTypes on invoice.payment_type_id equals payment_type.id
                                    join bank in db.Banks on invoice.bank_id equals bank.id
                                    join employee in db.Employees on invoice.employee_id equals employee.id
                                    join document in db.Documents on invoice.document_id equals document.id
                                       select new InvoiceViewModel
                                       {
                                           id = invoice.id,
                                           string_payment_type = payment_type.name,
                                           payment_type_id = invoice.payment_type_id,
                                           bank_id = invoice.bank_id,
                                           employee_id = invoice.employee_id,
                                           document_id = invoice.document_id,
                                           payment_date = invoice.payment_date,
                                           amount = invoice.amount,
                                           string_bank_name = bank.name,
                                           invoice_number = invoice.invoice_number,
                                           string_employee_name = employee.name,
                                           string_document_name = document.name,
                                           vacation_from = invoice.vacation_from,
                                           vacation_to = invoice.vacation_to,
                                           created_at = invoice.created_at
                                       }).AsEnumerable().Select(s => new InvoiceViewModel
                                       {
                                           id = s.id,
                                           string_payment_type = s.string_payment_type,
                                           payment_type_id = s.payment_type_id,
                                           bank_id = s.bank_id,
                                           employee_id = s.employee_id,
                                           document_id = s.document_id,
                                           payment_date = s.payment_date,
                                           string_payment_date = ((DateTime)s.payment_date).ToString("yyyy-MM-dd"),
                                           amount = s.amount,
                                           string_bank_name = s.string_bank_name,
                                           invoice_number = s.invoice_number,
                                           string_employee_name = s.string_employee_name,
                                           string_document_name = s.string_document_name,
                                           vacation_from = s.vacation_from,
                                           string_vacation_from = ((DateTime)s.vacation_from).ToString("yyyy-MM-dd"),
                                           vacation_to = s.vacation_to,
                                           string_vacation_to = ((DateTime)s.vacation_to).ToString("yyyy-MM-dd"),
                                           string_created_at = ((DateTime)s.created_at).ToString("yyyy-MM-dd")
                                       });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    invoicenData = invoicenData.Where(m => m.string_payment_type.ToLower().Contains(searchValue.ToLower())
                    || m.amount.ToString().ToLower().Contains(searchValue.ToLower())
                    || m.string_bank_name.ToString().ToLower().Contains(searchValue.ToLower())
                    || m.invoice_number.ToString().ToLower().Contains(searchValue.ToLower())
                    || m.string_employee_name.ToString().ToLower().Contains(searchValue.ToLower())
                    || m.string_document_name.ToString().ToLower().Contains(searchValue.ToLower())
                    || m.string_vacation_from.ToString().ToLower().Contains(searchValue.ToLower())
                    || m.string_vacation_to.ToString().ToLower().Contains(searchValue.ToLower())
                    || m.string_created_at.ToString().ToLower().Contains(searchValue.ToLower())
                    );
                }

                //total number of rows count     
                var displayResult = invoicenData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = invoicenData.Count();

                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = displayResult

                }, JsonRequestBehavior.AllowGet);

            }

            ViewBag.payment_types = db.PaymentTypes.Select(d => new { d.id, d.name }).ToList();
            ViewBag.banks = db.Banks.Select(d => new { d.id, d.name }).ToList();
            ViewBag.employees = db.Employees.Select(d => new { d.id, d.name }).ToList();
            ViewBag.documents = db.Documents.Select(d => new { d.id, d.name }).ToList();
            return View();
        }
        [HttpPost]
        public JsonResult saveInvoice(InvoiceViewModel invoiceVM)
        {
            if (invoiceVM.id == 0)
            {
                Invoice invoice = AutoMapper.Mapper.Map<InvoiceViewModel, Invoice>(invoiceVM);

                invoice.updated_at = DateTime.Now;
                invoice.created_at = DateTime.Now;

                db.Invoices.Add(invoice);
            }
            else
            {
                Invoice oldInvoice = db.Invoices.Find(invoiceVM.id);

                oldInvoice.payment_type_id = invoiceVM.payment_type_id;
                oldInvoice.payment_date = invoiceVM.payment_date;
                oldInvoice.amount = invoiceVM.amount;
                oldInvoice.bank_id = invoiceVM.bank_id;
                oldInvoice.invoice_number = invoiceVM.invoice_number;
                oldInvoice.employee_id = invoiceVM.employee_id;
                oldInvoice.document_id = invoiceVM.document_id;
                oldInvoice.vacation_from = invoiceVM.vacation_from;
                oldInvoice.vacation_to = invoiceVM.vacation_to;
                oldInvoice.updated_at = DateTime.Now;

                db.Entry(oldInvoice).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteInvoice(int id)
        {
            Invoice deleteInvoicen = db.Invoices.Find(id);
            db.Invoices.Remove(deleteInvoicen);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult checkPaymentType(int id)
        {
            PaymentType paymentType = db.PaymentTypes.Find(id);
            int hasFromTo = paymentType.from_to == 1 ? 1 : 0;
            return Json(new { hasFromTo = hasFromTo }, JsonRequestBehavior.AllowGet);
        }
    }
}