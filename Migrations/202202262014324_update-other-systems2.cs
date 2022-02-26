namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateothersystems2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeOtherSystems",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        employee_id = c.Int(),
                        other_system_id = c.Int(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Employees", t => t.employee_id)
                .ForeignKey("dbo.OtherSystems", t => t.other_system_id)
                .Index(t => t.employee_id)
                .Index(t => t.other_system_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeOtherSystems", "other_system_id", "dbo.OtherSystems");
            DropForeignKey("dbo.EmployeeOtherSystems", "employee_id", "dbo.Employees");
            DropIndex("dbo.EmployeeOtherSystems", new[] { "other_system_id" });
            DropIndex("dbo.EmployeeOtherSystems", new[] { "employee_id" });
            DropTable("dbo.EmployeeOtherSystems");
        }
    }
}
