namespace DependencySummary.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedTeamCityBuildId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Components", "TeamCityBuildId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Components", "TeamCityBuildId");
        }
    }
}
