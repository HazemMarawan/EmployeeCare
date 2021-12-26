namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTables5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        payment_type_id = c.Int(),
                        payment_date = c.DateTime(),
                        amount = c.Double(),
                        bank_id = c.Int(),
                        invoice_number = c.String(),
                        employee_id = c.Int(),
                        document_id = c.Int(),
                        vacation_from = c.DateTime(),
                        vacation_to = c.DateTime(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Banks", t => t.bank_id)
                .ForeignKey("dbo.Documents", t => t.document_id)
                .ForeignKey("dbo.Employees", t => t.employee_id)
                .ForeignKey("dbo.PaymentTypes", t => t.payment_type_id)
                .Index(t => t.payment_type_id)
                .Index(t => t.bank_id)
                .Index(t => t.employee_id)
                .Index(t => t.document_id);
            
            AddColumn("dbo.PaymentForms", "type", c => c.Int());
            AddColumn("dbo.PaymentForms", "record_date", c => c.DateTime());
            AddColumn("dbo.PaymentForms", "record_number", c => c.Int());
            AddColumn("dbo.PaymentForms", "collected_installments", c => c.Int());
            AddColumn("dbo.PaymentForms", "due_date", c => c.DateTime());
            AddColumn("dbo.PaymentForms", "last_installment", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "payment_type_id", "dbo.PaymentTypes");
            DropForeignKey("dbo.Invoices", "employee_id", "dbo.Employees");
            DropForeignKey("dbo.Invoices", "document_id", "dbo.Documents");
            DropForeignKey("dbo.Invoices", "bank_id", "dbo.Banks");
            DropIndex("dbo.Invoices", new[] { "document_id" });
            DropIndex("dbo.Invoices", new[] { "employee_id" });
            DropIndex("dbo.Invoices", new[] { "bank_id" });
            DropIndex("dbo.Invoices", new[] { "payment_type_id" });
            DropColumn("dbo.PaymentForms", "last_installment");
            DropColumn("dbo.PaymentForms", "due_date");
            DropColumn("dbo.PaymentForms", "collected_installments");
            DropColumn("dbo.PaymentForms", "record_number");
            DropColumn("dbo.PaymentForms", "record_date");
            DropColumn("dbo.PaymentForms", "type");
            DropTable("dbo.Invoices");
        }
    }
}
