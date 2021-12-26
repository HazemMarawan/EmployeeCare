namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTables21 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PaymentForms", "installments", c => c.Double());
            AlterColumn("dbo.Deductions", "installments", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Deductions", "installments", c => c.Int());
            AlterColumn("dbo.PaymentForms", "installments", c => c.Int());
        }
    }
}
