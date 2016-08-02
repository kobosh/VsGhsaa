namespace Ghsaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attendees : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MyEvents", "Attendees_Id", "dbo.Attendees");
            DropIndex("dbo.MyEvents", new[] { "Attendees_Id" });
            AddColumn("dbo.Attendees", "MyEvent_Id", c => c.Int());
            CreateIndex("dbo.Attendees", "MyEvent_Id");
            AddForeignKey("dbo.Attendees", "MyEvent_Id", "dbo.MyEvents", "Id");
            DropColumn("dbo.MyEvents", "Attendees_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MyEvents", "Attendees_Id", c => c.Int());
            DropForeignKey("dbo.Attendees", "MyEvent_Id", "dbo.MyEvents");
            DropIndex("dbo.Attendees", new[] { "MyEvent_Id" });
            DropColumn("dbo.Attendees", "MyEvent_Id");
            CreateIndex("dbo.MyEvents", "Attendees_Id");
            AddForeignKey("dbo.MyEvents", "Attendees_Id", "dbo.Attendees", "Id");
        }
    }
}
