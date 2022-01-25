namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentFormReport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentFormTasfyaReports",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        subscription_date = c.DateTime(),
                        salary = c.Double(),
                        discount_percentage = c.Double(),
                        reserved_months = c.Double(),
                        total = c.Double(),
                        payment_form_id = c.Int(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.PaymentForms", t => t.payment_form_id)
                .Index(t => t.payment_form_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentFormTasfyaReports", "payment_form_id", "dbo.PaymentForms");
            DropIndex("dbo.PaymentFormTasfyaReports", new[] { "payment_form_id" });
            DropTable("dbo.PaymentFormTasfyaReports");
        }
    }
}
