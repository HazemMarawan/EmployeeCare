namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDecisions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DecisionDocuments", "path", c => c.String());
            DropColumn("dbo.DecisionDocuments", "parh");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DecisionDocuments", "parh", c => c.String());
            DropColumn("dbo.DecisionDocuments", "path");
        }
    }
}
