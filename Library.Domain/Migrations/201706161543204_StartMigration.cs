namespace Library.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StartMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Author = c.String(),
                        Publishing = c.String(),
                        Year = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Description = c.String(),
                        Category = c.String(),
                        ImageData = c.Binary(),
                        ImageMimeType = c.String(),
                    })
                .PrimaryKey(t => t.BookId);
        }
        
        public override void Down()
        {
            DropTable("dbo.Books");
        }
    }
}