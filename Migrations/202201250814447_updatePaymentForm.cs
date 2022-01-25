namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePaymentForm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentTypes", "from_to", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentTypes", "from_to");
        }
    }
}
