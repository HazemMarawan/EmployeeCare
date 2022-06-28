namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class legan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.a3da2Legan",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        employee_id = c.Int(),
                        status = c.String(),
                        date = c.String(),
                        date_submitted = c.String(),
                        legan_mosa3dat_id = c.Int(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Employees", t => t.employee_id)
                .ForeignKey("dbo.leganMosa3dat", t => t.legan_mosa3dat_id)
                .Index(t => t.employee_id)
                .Index(t => t.legan_mosa3dat_id);
            
            CreateTable(
                "dbo.leganMosa3dat",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        lagna_at = c.DateTime(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.a3da2Legan", "legan_mosa3dat_id", "dbo.leganMosa3dat");
            DropForeignKey("dbo.a3da2Legan", "employee_id", "dbo.Employees");
            DropIndex("dbo.a3da2Legan", new[] { "legan_mosa3dat_id" });
            DropIndex("dbo.a3da2Legan", new[] { "employee_id" });
            DropTable("dbo.leganMosa3dat");
            DropTable("dbo.a3da2Legan");
        }
    }
}
