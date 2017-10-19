namespace Library.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Details",
                c => new
                    {
                        DetailId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Number = c.String(nullable: false),
                        Date = c.String(nullable: false),
                        Time = c.String(nullable: false),
                        Orders_OrderId = c.Int(),
                    })
                .PrimaryKey(t => t.DetailId)
                .ForeignKey("dbo.Orders", t => t.Orders_OrderId)
                .Index(t => t.Orders_OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateTable(
                "dbo.OrderLines",
                c => new
                    {
                        OrderLineId = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Book_BookId = c.Int(),
                        Order_OrderId = c.Int(),
                    })
                .PrimaryKey(t => t.OrderLineId)
                .ForeignKey("dbo.Books", t => t.Book_BookId)
                .ForeignKey("dbo.Orders", t => t.Order_OrderId)
                .Index(t => t.Book_BookId)
                .Index(t => t.Order_OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Details", "Orders_OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderLines", "Order_OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderLines", "Book_BookId", "dbo.Books");
            DropIndex("dbo.OrderLines", new[] { "Order_OrderId" });
            DropIndex("dbo.OrderLines", new[] { "Book_BookId" });
            DropIndex("dbo.Details", new[] { "Orders_OrderId" });
            DropTable("dbo.OrderLines");
            DropTable("dbo.Orders");
            DropTable("dbo.Details");
        }
    }
}
