namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class accounting_tree : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountingTrees",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        code = c.String(),
                        level1 = c.String(),
                        level2 = c.String(),
                        range_from = c.String(),
                        range_to = c.String(),
                        level = c.String(),
                        type = c.Int(),
                        mden_da2en = c.Int(),
                        parent_id = c.Int(),
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
            DropTable("dbo.AccountingTrees");
        }
    }
}
