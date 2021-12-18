namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUserTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "nationality");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "nationality", c => c.String());
        }
    }
}
