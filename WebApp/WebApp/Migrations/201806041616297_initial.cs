namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        userName = c.String(),
                        orderTime = c.DateTime(nullable: false),
                        total = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        ProductType = c.String(),
                        cost = c.Double(nullable: false),
                        ImageURL = c.String(),
                        Description = c.String(),
                        Amount = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        OrderDetails_OrderID = c.Int(),
                        User_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .ForeignKey("dbo.OrderDetails", t => t.OrderDetails_OrderID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.SupplierId)
                .Index(t => t.OrderDetails_OrderID)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StorageProducts",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        productName = c.String(),
                        lastOrder = c.DateTime(nullable: false),
                        amount = c.Int(nullable: false),
                        Supplier_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Suppliers", t => t.Supplier_ID)
                .Index(t => t.Supplier_ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        Mail = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "User_ID", "dbo.Users");
            DropForeignKey("dbo.StorageProducts", "Supplier_ID", "dbo.Suppliers");
            DropForeignKey("dbo.Products", "OrderDetails_OrderID", "dbo.OrderDetails");
            DropForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers");
            DropIndex("dbo.StorageProducts", new[] { "Supplier_ID" });
            DropIndex("dbo.Products", new[] { "User_ID" });
            DropIndex("dbo.Products", new[] { "OrderDetails_OrderID" });
            DropIndex("dbo.Products", new[] { "SupplierId" });
            DropTable("dbo.Users");
            DropTable("dbo.StorageProducts");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Products");
            DropTable("dbo.OrderDetails");
        }
    }
}
