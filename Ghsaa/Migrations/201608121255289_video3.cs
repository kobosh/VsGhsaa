namespace Ghsaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class video3 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Videos");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        UploadDate = c.DateTime(nullable: false),
                        Picture = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
