namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "users_access", c => c.Int());
            AddColumn("dbo.Users", "primary_data_access", c => c.Int());
            AddColumn("dbo.Users", "payment_forms_management_access", c => c.Int());
            AddColumn("dbo.Users", "manage_inventory_access", c => c.Int());
            AddColumn("dbo.Users", "accountants_access", c => c.Int());
            AddColumn("dbo.Users", "paid_off_access", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "paid_off_access");
            DropColumn("dbo.Users", "accountants_access");
            DropColumn("dbo.Users", "manage_inventory_access");
            DropColumn("dbo.Users", "payment_forms_management_access");
            DropColumn("dbo.Users", "primary_data_access");
            DropColumn("dbo.Users", "users_access");
        }
    }
}
