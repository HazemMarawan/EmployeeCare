namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTables3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PaymentForms", "cheque_cost", c => c.Double());
            AlterColumn("dbo.Deductions", "cheque_cost", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Deductions", "cheque_cost", c => c.Int());
            AlterColumn("dbo.PaymentForms", "cheque_cost", c => c.Int());
        }
    }
}
