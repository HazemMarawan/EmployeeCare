namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTables1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deductions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        managerial_fees = c.Double(),
                        installments = c.Int(),
                        cheque_cost = c.Int(),
                        other_income = c.Double(),
                        total_deduction = c.Double(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Deductions");
        }
    }
}
