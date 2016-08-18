namespace Ghsaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class video31 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        UploadDate = c.DateTime(nullable: false),
                        Picture = c.String(),
                        TypeOfVideo = c.String(),
                        bytes = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Videos");
        }
    }
}
