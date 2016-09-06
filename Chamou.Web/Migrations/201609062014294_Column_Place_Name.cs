namespace Chamou.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Column_Place_Name : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Places", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Places", "Name");
        }
    }
}
