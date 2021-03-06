using System;
using System.Data.Entity;
using System.Linq;

namespace EmployeeCare.Models
{
    public class EmployeeCareDbContext : DbContext
    {
        // Your context has been configured to use a 'EmployeeCareDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'EmployeeCare.Models.EmployeeCareDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'EmployeeCareDbContext' 
        // connection string in the application configuration file.
        public EmployeeCareDbContext()
            : base("name=EmployeeCareDbContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

         public virtual DbSet<Decision> Decisions { get; set; }
         public virtual DbSet<DecisionDocument> DecisionDocuments { get; set; }
         public virtual DbSet<Destination> Destinations { get; set; }
         public virtual DbSet<Document> Documents { get; set; }
         public virtual DbSet<Employee> Employees { get; set; }
         public virtual DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
         public virtual DbSet<Grade> Grades { get; set; }
         public virtual DbSet<PaymentForm> PaymentForms { get; set; }
         public virtual DbSet<PaymentType> PaymentTypes { get; set; }
         public virtual DbSet<User> Users { get; set; }
         public virtual DbSet<Bank> Banks { get; set; }
         public virtual DbSet<EmployeeArchive> EmployeeArchives { get; set; }
         public virtual DbSet<Deduction> Deductions { get; set; }
         public virtual DbSet<Setting> Settings { get; set; }
         public virtual DbSet<Invoice> Invoices { get; set; }
         public virtual DbSet<PaymentFormTasfyaReport> PaymentFormTasfyaReports { get; set; }
         public virtual DbSet<OtherSystem> OtherSystems { get; set; }
         public virtual DbSet<EmployeeOtherSystem> EmployeeOtherSystems { get; set; }
         public virtual DbSet<AccountingTree> AccountingTrees { get; set; }
         public virtual DbSet<leganMosa3dat> LeganMosa3Dats { get; set; }
         public virtual DbSet<a3da2Legan> A3Da2Legans { get; set; }
    }
}