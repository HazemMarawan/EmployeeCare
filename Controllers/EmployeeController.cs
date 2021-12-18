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
    public class EmployeeController : Controller
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
                var destinationData = (from employee in db.Employees
                                       join destination in db.Destinations on employee.destination_id equals destination.id
                                       join grade in db.Grades on employee.grade_id equals grade.id
                                       join bank in db.Banks on employee.bank_id equals bank.id
                                       select new EmployeeViewModel
                                       {
                                           id = employee.id,
                                           name = employee.name,
                                           code = employee.code,
                                           destination_id = employee.destination_id,
                                           grade_id = employee.grade_id,
                                           bank_id = employee.bank_id,
                                           destination_name = destination.name,
                                           grade_name = grade.name,
                                           national_id = employee.national_id,
                                           employee_file_number = employee.employee_file_number,
                                           email = employee.email,
                                           employee_status = employee.status,
                                           membership_status = employee.membership_status,
                                           bank_account_number = employee.bank_account_number,
                                           bank_name = bank.name,
                                           active = employee.active,
                                           created_at = employee.created_at
                                       }).AsEnumerable().Select(s => new EmployeeViewModel
                                       {
                                           id = s.id,
                                           name = s.name,
                                           code = s.code,
                                           destination_id = s.destination_id,
                                           grade_id = s.grade_id,
                                           bank_id = s.bank_id,
                                           destination_name = s.destination_name,
                                           grade_name = s.grade_name,
                                           national_id = s.national_id,
                                           employee_file_number = s.employee_file_number,
                                           email = s.email,
                                           employee_status = s.employee_status,
                                           membership_status = s.membership_status,
                                           bank_account_number = s.bank_account_number,
                                           bank_name = s.bank_name,
                                           active = s.active,
                                           string_created_at = ((DateTime)s.created_at).ToString("yyyy-MM-dd")
                                       });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    destinationData = destinationData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) 
                        || m.id.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.code.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.destination_name.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.grade_name.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.national_id.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.email.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.bank_account_number.ToString().ToLower().Contains(searchValue.ToLower())
                        || m.bank_name.ToString().ToLower().Contains(searchValue.ToLower())
                       );
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
            ViewBag.destinations = db.Destinations.Select(d => new { d.id, d.name }).ToList();
            ViewBag.grades = db.Grades.Select(d => new { d.id, d.name }).ToList();
            ViewBag.banks = db.Banks.Select(d => new { d.id, d.name }).ToList();

            return View();
        }
        [HttpPost]
        public JsonResult saveEmployee(EmployeeViewModel employeeVM)
        {
            if (employeeVM.id == 0)
            {
                Employee employee = AutoMapper.Mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                employee.updated_at = DateTime.Now;
                employee.created_at = DateTime.Now;

                db.Employees.Add(employee);
            }
            else
            {
                Employee oldEmployee = db.Employees.Find(employeeVM.id);

                oldEmployee.name = employeeVM.name;
                oldEmployee.code = employeeVM.code;
                oldEmployee.destination_id = employeeVM.destination_id;
                oldEmployee.grade_id = employeeVM.grade_id;
                oldEmployee.national_id = employeeVM.national_id;
                oldEmployee.email = employeeVM.email;
                oldEmployee.status = employeeVM.status;
                oldEmployee.membership_status = employeeVM.membership_status;
                oldEmployee.bank_account_number = employeeVM.bank_account_number;
                oldEmployee.bank_id = employeeVM.bank_id;
                oldEmployee.active = employeeVM.active;
                oldEmployee.updated_at = DateTime.Now;

                db.Entry(oldEmployee).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteEmployee(int id)
        {
            Employee deleteEmployee = db.Employees.Find(id);
            db.Employees.Remove(deleteEmployee);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }
    }
}