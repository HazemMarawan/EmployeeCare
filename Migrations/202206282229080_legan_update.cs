namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class legan_update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.a3da2Legan", "cheque_number", c => c.String());
            AddColumn("dbo.a3da2Legan", "bank_id", c => c.Int());
            AlterColumn("dbo.a3da2Legan", "date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.a3da2Legan", "date_submitted", c => c.DateTime(nullable: false));
            CreateIndex("dbo.a3da2Legan", "bank_id");
            AddForeignKey("dbo.a3da2Legan", "bank_id", "dbo.Banks", "id");
            DropColumn("dbo.a3da2Legan", "name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.a3da2Legan", "name", c => c.String());
            DropForeignKey("dbo.a3da2Legan", "bank_id", "dbo.Banks");
            DropIndex("dbo.a3da2Legan", new[] { "bank_id" });
            AlterColumn("dbo.a3da2Legan", "date_submitted", c => c.String());
            AlterColumn("dbo.a3da2Legan", "date", c => c.String());
            DropColumn("dbo.a3da2Legan", "bank_id");
            DropColumn("dbo.a3da2Legan", "cheque_number");
        }
    }
}
