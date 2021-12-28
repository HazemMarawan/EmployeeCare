namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_subscription_date_payment_form : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentForms", "subscription_date", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentForms", "subscription_date");
        }
    }
}
