namespace Ghsaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attendees", "MyEvent_Id", "dbo.MyEvents");
            DropIndex("dbo.Attendees", new[] { "MyEvent_Id" });
            AddColumn("dbo.Attendees", "MyEventId", c => c.Int(nullable: false));
            DropColumn("dbo.Attendees", "MyEvent_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Attendees", "MyEvent_Id", c => c.Int());
            DropColumn("dbo.Attendees", "MyEventId");
            CreateIndex("dbo.Attendees", "MyEvent_Id");
            AddForeignKey("dbo.Attendees", "MyEvent_Id", "dbo.MyEvents", "Id");
        }
    }
}
