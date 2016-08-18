namespace Ghsaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videos2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "Picture", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "Picture");
        }
    }
}
