namespace DependencySummary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactoredForeignKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Packages", "Component_Id", "dbo.Components");
            DropForeignKey("dbo.Projects", "Package_Id", "dbo.Packages");
            DropIndex("dbo.Packages", new[] { "Component_Id" });
            DropIndex("dbo.Projects", new[] { "Package_Id" });
            RenameColumn(table: "dbo.Packages", name: "Component_Id", newName: "ComponentId");
            RenameColumn(table: "dbo.Projects", name: "Package_Id", newName: "PackageId");
            AlterColumn("dbo.Packages", "ComponentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Projects", "PackageId", c => c.Int(nullable: false));
            CreateIndex("dbo.Packages", "ComponentId");
            CreateIndex("dbo.Projects", "PackageId");
            AddForeignKey("dbo.Packages", "ComponentId", "dbo.Components", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Projects", "PackageId", "dbo.Packages", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "PackageId", "dbo.Packages");
            DropForeignKey("dbo.Packages", "ComponentId", "dbo.Components");
            DropIndex("dbo.Projects", new[] { "PackageId" });
            DropIndex("dbo.Packages", new[] { "ComponentId" });
            AlterColumn("dbo.Projects", "PackageId", c => c.Int());
            AlterColumn("dbo.Packages", "ComponentId", c => c.Int());
            RenameColumn(table: "dbo.Projects", name: "PackageId", newName: "Package_Id");
            RenameColumn(table: "dbo.Packages", name: "ComponentId", newName: "Component_Id");
            CreateIndex("dbo.Projects", "Package_Id");
            CreateIndex("dbo.Packages", "Component_Id");
            AddForeignKey("dbo.Projects", "Package_Id", "dbo.Packages", "Id");
            AddForeignKey("dbo.Packages", "Component_Id", "dbo.Components", "Id");
        }
    }
}
