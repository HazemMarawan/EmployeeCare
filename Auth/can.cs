using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeCare.Models;

namespace EmployeeCare.Auth
{
    public class can
    {
        
        public static bool usersAccess()
        {
            User user = HttpContext.Current.Session["user"] as User;
            if (user.users_access == 1)
                return true;
            return false;
        }
        public static bool primaryDataAccess()
        {
            User user = HttpContext.Current.Session["user"] as User;
            if (user.primary_data_access == 1)
                return true;
            return false;
        }
        public static bool paymentFormsManagementAccess()
        {
            User user = HttpContext.Current.Session["user"] as User;
            if (user.payment_forms_management_access == 1)
                return true;
            return false;
        }
        public static bool manageInventoryAccess()
        {
            User user = HttpContext.Current.Session["user"] as User;
            if (user.manage_inventory_access == 1)
                return true;
            return false;
        }
        public static bool accountantsAccess()
        {
            User user = HttpContext.Current.Session["user"] as User;
            if (user.accountants_access == 1)
                return true;
            return false;
        }
        public static bool paidOffAccess()
        {
            User user = HttpContext.Current.Session["user"] as User;
            if (user.paid_off_access == 1)
                return true;
            return false;
        }
    }
}