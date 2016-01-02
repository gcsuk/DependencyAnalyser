namespace DependencySummary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Components",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Version = c.String(),
                        TargetFramework = c.String(),
                        Component_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Components", t => t.Component_Id)
                .Index(t => t.Component_Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Package_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Packages", t => t.Package_Id)
                .Index(t => t.Package_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Packages", "Component_Id", "dbo.Components");
            DropForeignKey("dbo.Projects", "Package_Id", "dbo.Packages");
            DropIndex("dbo.Projects", new[] { "Package_Id" });
            DropIndex("dbo.Packages", new[] { "Component_Id" });
            DropTable("dbo.Projects");
            DropTable("dbo.Packages");
            DropTable("dbo.Components");
        }
    }
}
