namespace Chamou.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Attendant : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        PlaceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Places", t => t.PlaceId, cascadeDelete: true)
                .Index(t => t.PlaceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendants", "PlaceId", "dbo.Places");
            DropIndex("dbo.Attendants", new[] { "PlaceId" });
            DropTable("dbo.Attendants");
        }
    }
}
