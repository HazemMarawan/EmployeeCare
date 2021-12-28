namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_payment_form : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PaymentForms", "record_no", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PaymentForms", "record_no", c => c.Int(nullable: false));
        }
    }
}
