namespace DependencySummary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLastUpdatedProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Packages", "LastUpdated", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Packages", "LastUpdated");
        }
    }
}
