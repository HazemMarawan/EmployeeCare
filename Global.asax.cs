using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EmployeeCare.Models;
using EmployeeCare.ViewModel;

namespace EmployeeCare
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.CreateMap<DestinationViewModel, Destination>();
            Mapper.CreateMap<GradeViewModel, Grade>();
            Mapper.CreateMap<DocumentViewModel, Document>();
            Mapper.CreateMap<PaymentTypeViewModel, PaymentType>();
            Mapper.CreateMap<UserViewModel, User>();
            Mapper.CreateMap<EmployeeViewModel, Employee>();
            Mapper.CreateMap<BankViewModel, Bank>();
            Mapper.CreateMap<EmployeeDocumentViewModel, EmployeeDocument>();
            Mapper.CreateMap<PaymentFormViewModel, PaymentForm>();
            Mapper.CreateMap<DecisionViewModel, Decision>();
            Mapper.CreateMap<InvoiceViewModel, Invoice>();
        }
    }
}
