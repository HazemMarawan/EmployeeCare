using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeCare.Enum
{
    public enum Gender
    {
        Male = 1,
        Female = 2,
    }
    public enum UserType
    {
        Admin = 1,
        docmentsUser = 2,
        accountant = 3,
    }
    public enum UserActiveStatus
    {
        Active = 1,
        NotActive = 2,
    }
    public enum PaymentFormTypes
    {
        Takaful = 1,
        Tasfya = 2
    }
}