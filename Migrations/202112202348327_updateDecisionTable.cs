namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDecisionTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Decisions", "decision_type", c => c.Int());
            DropColumn("dbo.Decisions", "destination_type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Decisions", "destination_type", c => c.Int());
            DropColumn("dbo.Decisions", "decision_type");
        }
    }
}
