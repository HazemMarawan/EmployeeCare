namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_payment_form : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PaymentForms", "collected_installments", c => c.Double());
            DropColumn("dbo.PaymentForms", "record_number");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentForms", "record_number", c => c.Int());
            AlterColumn("dbo.PaymentForms", "collected_installments", c => c.Int());
        }
    }
}
