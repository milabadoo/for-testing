namespace Library.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRaiting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        RatingId = c.Int(nullable: false, identity: true),
                        RatingCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RatingId);
            
            AddColumn("dbo.Books", "Rating_RatingId", c => c.Int());
            CreateIndex("dbo.Books", "Rating_RatingId");
            AddForeignKey("dbo.Books", "Rating_RatingId", "dbo.Ratings", "RatingId");
            DropColumn("dbo.Books", "Rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "Rating", c => c.Int(nullable: false));
            DropForeignKey("dbo.Books", "Rating_RatingId", "dbo.Ratings");
            DropIndex("dbo.Books", new[] { "Rating_RatingId" });
            DropColumn("dbo.Books", "Rating_RatingId");
            DropTable("dbo.Ratings");
        }
    }
}
