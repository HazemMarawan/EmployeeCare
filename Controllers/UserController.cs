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
    public class UserController : Controller
    {
        EmployeeCareDbContext db = new EmployeeCareDbContext();
        // GET: User
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
                var userData = (from user in db.Users
                                select new UserViewModel
                                {
                                    id = user.id,
                                    user_name = user.user_name,
                                    full_name = user.full_name,
                                    password = user.password,
                                    type = user.type,
                                    phone = user.phone,
                                    address = user.address,
                                    birthDate = user.birthDate,
                                    imagePath = user.image,
                                    code = user.code,
                                    email = user.email,
                                    gender = user.gender,
                                    active = user.active,
                                });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = userData.Where(m => m.full_name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.email.ToLower().Contains(searchValue.ToLower()) || m.user_name.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = userData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = userData.Count();

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
        public JsonResult saveUser(UserViewModel userVM)
        {
            User currentUser = Session["user"] as User;

            if (userVM.id == 0)
            {
                User user = AutoMapper.Mapper.Map<UserViewModel, User>(userVM);

                user.created_at = DateTime.Now;
                user.updated_at = DateTime.Now;
                user.created_by = currentUser.id;
                //user.type = (int)UserTypes.Staff;

                if (userVM.image != null)
                {
                    Guid guid = Guid.NewGuid();
                    var InputFileName = Path.GetFileName(userVM.image.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Uploads/Profiles/") + guid.ToString() + "_Profile" + Path.GetExtension(userVM.image.FileName));
                    userVM.image.SaveAs(ServerSavePath);
                    user.image = "/Uploads/Profiles/" + guid.ToString() + "_Profile" + Path.GetExtension(userVM.image.FileName);
                }
                db.Users.Add(user);
                db.SaveChanges();
            }
            else
            {

                User oldUser = db.Users.Find(userVM.id);

                oldUser.full_name = userVM.full_name;
                oldUser.user_name = userVM.user_name;
                oldUser.password = userVM.password;
                oldUser.code = userVM.code;
                oldUser.phone = userVM.phone;
                oldUser.address = userVM.address;
                oldUser.email = userVM.email;
                oldUser.birthDate = userVM.birthDate;
                oldUser.gender = userVM.gender;
                oldUser.type = userVM.type;
                oldUser.active = userVM.active;
                if (userVM.image != null)
                {
                    Guid guid = Guid.NewGuid();
                    var InputFileName = Path.GetFileName(userVM.image.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Uploads/Profiles/") + guid.ToString() + "_Profile" + Path.GetExtension(userVM.image.FileName));
                    userVM.image.SaveAs(ServerSavePath);
                    oldUser.image = "/Uploads/Profiles/" + guid.ToString() + "_Profile" + Path.GetExtension(userVM.image.FileName);
                }
                oldUser.updated_at = DateTime.Now;
                db.Entry(oldUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteUser(int id)
        {

            User deleteUser = db.Users.Find(id);
            db.Users.Remove(deleteUser);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult checkUsernameAvailability(string user_name, int id = 0)
        {
            if (id != 0)
            {
                var oldUsername = db.Users.Find(id).user_name;
                if (oldUsername == user_name)
                    return Json(new { message = "valid username", is_valid = true }, JsonRequestBehavior.AllowGet);

            }
            var checkAvailabilty = db.Users.Any(s => s.user_name == user_name);
            if (checkAvailabilty)
            {
                return Json(new { message = "username already taken", is_valid = false }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { message = "valid username", is_valid = true }, JsonRequestBehavior.AllowGet);
        }

    }
}