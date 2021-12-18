namespace EmployeeCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDocumentsTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DocumentTypes", newName: "Documents");
            RenameColumn(table: "dbo.EmployeeDocuments", name: "document_type_id", newName: "document_id");
            RenameIndex(table: "dbo.EmployeeDocuments", name: "IX_document_type_id", newName: "IX_document_id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.EmployeeDocuments", name: "IX_document_id", newName: "IX_document_type_id");
            RenameColumn(table: "dbo.EmployeeDocuments", name: "document_id", newName: "document_type_id");
            RenameTable(name: "dbo.Documents", newName: "DocumentTypes");
        }
    }
}
