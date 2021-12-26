namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTables4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentForms", "subscription", c => c.Double());
            AddColumn("dbo.Deductions", "subscription", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Deductions", "subscription");
            DropColumn("dbo.PaymentForms", "subscription");
        }
    }
}
