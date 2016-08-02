namespace Ghsaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attendees", "MyEvent_Id", "dbo.MyEvents");
            DropIndex("dbo.Attendees", new[] { "MyEvent_Id" });
            RenameColumn(table: "dbo.Attendees", name: "MyEvent_Id", newName: "MyEventRefId");
            AlterColumn("dbo.Attendees", "MyEventRefId", c => c.Int(nullable: false));
            CreateIndex("dbo.Attendees", "MyEventRefId");
            AddForeignKey("dbo.Attendees", "MyEventRefId", "dbo.MyEvents", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendees", "MyEventRefId", "dbo.MyEvents");
            DropIndex("dbo.Attendees", new[] { "MyEventRefId" });
            AlterColumn("dbo.Attendees", "MyEventRefId", c => c.Int());
            RenameColumn(table: "dbo.Attendees", name: "MyEventRefId", newName: "MyEvent_Id");
            CreateIndex("dbo.Attendees", "MyEvent_Id");
            AddForeignKey("dbo.Attendees", "MyEvent_Id", "dbo.MyEvents", "Id");
        }
    }
}
