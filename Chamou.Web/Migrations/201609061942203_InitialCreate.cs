namespace Chamou.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GeoPoints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Place_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Places", t => t.Place_Id)
                .Index(t => t.Place_Id);
            
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Location = c.Geography(nullable: false),
                        CenterLatitude = c.Double(nullable: false),
                        CenterLongitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GeoPoints", "Place_Id", "dbo.Places");
            DropIndex("dbo.GeoPoints", new[] { "Place_Id" });
            DropTable("dbo.Places");
            DropTable("dbo.GeoPoints");
        }
    }
}
