namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migPaymentForm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentForms", "record_no", c => c.Int(nullable: false));
            AddColumn("dbo.PaymentForms", "details", c => c.String());
            AddColumn("dbo.PaymentForms", "reason_est7kak", c => c.String());
            AddColumn("dbo.PaymentForms", "date_est7kak", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentForms", "date_est7kak");
            DropColumn("dbo.PaymentForms", "reason_est7kak");
            DropColumn("dbo.PaymentForms", "details");
            DropColumn("dbo.PaymentForms", "record_no");
        }
    }
}
