namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateothersystems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OtherSystems",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        OtherSystem_id = c.Int(),
                        Employee_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.OtherSystems", t => t.OtherSystem_id)
                .ForeignKey("dbo.Employees", t => t.Employee_id)
                .Index(t => t.OtherSystem_id)
                .Index(t => t.Employee_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OtherSystems", "Employee_id", "dbo.Employees");
            DropForeignKey("dbo.OtherSystems", "OtherSystem_id", "dbo.OtherSystems");
            DropIndex("dbo.OtherSystems", new[] { "Employee_id" });
            DropIndex("dbo.OtherSystems", new[] { "OtherSystem_id" });
            DropTable("dbo.OtherSystems");
        }
    }
}
