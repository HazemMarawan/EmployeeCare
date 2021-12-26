namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "birth_date", c => c.DateTime());
            AddColumn("dbo.PaymentForms", "managerial_fees", c => c.Double());
            AddColumn("dbo.PaymentForms", "installments", c => c.Int());
            AddColumn("dbo.PaymentForms", "cheque_cost", c => c.Int());
            AddColumn("dbo.PaymentForms", "other_income", c => c.Double());
            AddColumn("dbo.PaymentForms", "total_deduction", c => c.Double());
            AddColumn("dbo.PaymentForms", "cheque_number", c => c.String());
            AlterColumn("dbo.PaymentForms", "last_paid_installment", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PaymentForms", "last_paid_installment", c => c.Double());
            DropColumn("dbo.PaymentForms", "cheque_number");
            DropColumn("dbo.PaymentForms", "total_deduction");
            DropColumn("dbo.PaymentForms", "other_income");
            DropColumn("dbo.PaymentForms", "cheque_cost");
            DropColumn("dbo.PaymentForms", "installments");
            DropColumn("dbo.PaymentForms", "managerial_fees");
            DropColumn("dbo.Employees", "birth_date");
        }
    }
}
