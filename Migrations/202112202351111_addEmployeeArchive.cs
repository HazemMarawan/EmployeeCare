namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEmployeeArchive : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeArchives",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        employee_id = c.Int(),
                        path = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Employees", t => t.employee_id)
                .Index(t => t.employee_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeArchives", "employee_id", "dbo.Employees");
            DropIndex("dbo.EmployeeArchives", new[] { "employee_id" });
            DropTable("dbo.EmployeeArchives");
        }
    }
}
