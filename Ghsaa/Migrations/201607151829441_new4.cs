namespace Ghsaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attendees", "MyEventRefId", "dbo.MyEvents");
            DropIndex("dbo.Attendees", new[] { "MyEventRefId" });
            RenameColumn(table: "dbo.Attendees", name: "MyEventRefId", newName: "MyEvent_Id");
            AlterColumn("dbo.Attendees", "MyEvent_Id", c => c.Int());
            CreateIndex("dbo.Attendees", "MyEvent_Id");
            AddForeignKey("dbo.Attendees", "MyEvent_Id", "dbo.MyEvents", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendees", "MyEvent_Id", "dbo.MyEvents");
            DropIndex("dbo.Attendees", new[] { "MyEvent_Id" });
            AlterColumn("dbo.Attendees", "MyEvent_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Attendees", name: "MyEvent_Id", newName: "MyEventRefId");
            CreateIndex("dbo.Attendees", "MyEventRefId");
            AddForeignKey("dbo.Attendees", "MyEventRefId", "dbo.MyEvents", "Id", cascadeDelete: true);
        }
    }
}
