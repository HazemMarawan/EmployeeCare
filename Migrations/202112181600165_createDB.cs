namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DecisionDocuments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        parh = c.String(),
                        decision_id = c.Int(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Decisions", t => t.decision_id)
                .Index(t => t.decision_id);
            
            CreateTable(
                "dbo.Decisions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(),
                        destination_id = c.Int(),
                        title = c.String(),
                        decision_date = c.DateTime(),
                        destination_type = c.Int(),
                        notes = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Destinations", t => t.destination_id)
                .Index(t => t.destination_id);
            
            CreateTable(
                "dbo.Destinations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(),
                        name = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        destination_id = c.Int(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Destinations", t => t.destination_id)
                .Index(t => t.destination_id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(),
                        destination_id = c.Int(),
                        employee_file_number = c.Int(),
                        name = c.String(),
                        grade_id = c.Int(),
                        national_id = c.String(),
                        email = c.String(),
                        status = c.Int(),
                        membership_status = c.Int(),
                        bank_account_number = c.String(),
                        bank_id = c.Int(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Banks", t => t.bank_id)
                .ForeignKey("dbo.Destinations", t => t.destination_id)
                .ForeignKey("dbo.Grades", t => t.grade_id)
                .Index(t => t.destination_id)
                .Index(t => t.grade_id)
                .Index(t => t.bank_id);
            
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.EmployeeDocuments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        document_number = c.Int(),
                        employee_id = c.Int(),
                        document_type_id = c.Int(),
                        subscription_date = c.DateTime(),
                        percentage = c.Single(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.DocumentTypes", t => t.document_type_id)
                .ForeignKey("dbo.Employees", t => t.employee_id)
                .Index(t => t.employee_id)
                .Index(t => t.document_type_id);
            
            CreateTable(
                "dbo.PaymentForms",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(),
                        employee_document_id = c.Int(),
                        decision_id = c.Int(),
                        salary = c.Double(),
                        no_of_months = c.Double(),
                        last_paid_installment = c.Double(),
                        deduct_amount_from_takaful = c.Double(),
                        installment_need_deduct = c.Double(),
                        debt_need_deduct = c.Double(),
                        membership_subscription_deduct = c.Double(),
                        final_paid = c.Double(),
                        notes = c.String(),
                        approval_status = c.Int(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        Employee_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Decisions", t => t.decision_id)
                .ForeignKey("dbo.EmployeeDocuments", t => t.employee_document_id)
                .ForeignKey("dbo.Employees", t => t.Employee_id)
                .Index(t => t.employee_document_id)
                .Index(t => t.decision_id)
                .Index(t => t.Employee_id);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.PaymentTypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        code = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(),
                        user_name = c.String(),
                        full_name = c.String(),
                        email = c.String(),
                        password = c.String(),
                        phone = c.String(),
                        address = c.String(),
                        gender = c.Int(),
                        nationality = c.String(),
                        birthDate = c.DateTime(),
                        image = c.String(),
                        type = c.Int(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DecisionDocuments", "decision_id", "dbo.Decisions");
            DropForeignKey("dbo.Decisions", "destination_id", "dbo.Destinations");
            DropForeignKey("dbo.PaymentForms", "Employee_id", "dbo.Employees");
            DropForeignKey("dbo.Employees", "grade_id", "dbo.Grades");
            DropForeignKey("dbo.PaymentForms", "employee_document_id", "dbo.EmployeeDocuments");
            DropForeignKey("dbo.PaymentForms", "decision_id", "dbo.Decisions");
            DropForeignKey("dbo.EmployeeDocuments", "employee_id", "dbo.Employees");
            DropForeignKey("dbo.EmployeeDocuments", "document_type_id", "dbo.DocumentTypes");
            DropForeignKey("dbo.Employees", "destination_id", "dbo.Destinations");
            DropForeignKey("dbo.Employees", "bank_id", "dbo.Banks");
            DropForeignKey("dbo.DocumentTypes", "destination_id", "dbo.Destinations");
            DropIndex("dbo.PaymentForms", new[] { "Employee_id" });
            DropIndex("dbo.PaymentForms", new[] { "decision_id" });
            DropIndex("dbo.PaymentForms", new[] { "employee_document_id" });
            DropIndex("dbo.EmployeeDocuments", new[] { "document_type_id" });
            DropIndex("dbo.EmployeeDocuments", new[] { "employee_id" });
            DropIndex("dbo.Employees", new[] { "bank_id" });
            DropIndex("dbo.Employees", new[] { "grade_id" });
            DropIndex("dbo.Employees", new[] { "destination_id" });
            DropIndex("dbo.DocumentTypes", new[] { "destination_id" });
            DropIndex("dbo.Decisions", new[] { "destination_id" });
            DropIndex("dbo.DecisionDocuments", new[] { "decision_id" });
            DropTable("dbo.Users");
            DropTable("dbo.PaymentTypes");
            DropTable("dbo.Grades");
            DropTable("dbo.PaymentForms");
            DropTable("dbo.EmployeeDocuments");
            DropTable("dbo.Banks");
            DropTable("dbo.Employees");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.Destinations");
            DropTable("dbo.Decisions");
            DropTable("dbo.DecisionDocuments");
        }
    }
}
