namespace Phony.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(),
                        CompanyId = c.Int(),
                        ServiceId = c.Int(),
                        Discount = c.Decimal(precision: 18, scale: 2),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .ForeignKey("dbo.Services", t => t.ServiceId)
                .Index(t => t.ClientId)
                .Index(t => t.CompanyId)
                .Index(t => t.ServiceId)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
            CreateTable(
                "dbo.BillMoves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BillId = c.Int(),
                        ItemId = c.Int(),
                        QTY = c.Decimal(precision: 18, scale: 2),
                        ServiceId = c.Int(),
                        Discount = c.Decimal(precision: 18, scale: 2),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bills", t => t.BillId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .ForeignKey("dbo.Services", t => t.ServiceId)
                .Index(t => t.BillId)
                .Index(t => t.ItemId)
                .Index(t => t.ServiceId)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Pass = c.String(),
                        Group = c.Byte(nullable: false),
                        Phone = c.String(),
                        Notes = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Barcode = c.String(),
                        Shopcode = c.String(),
                        Image = c.Binary(storeType: "image"),
                        Group = c.Byte(nullable: false),
                        PurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WholeSalePrice = c.Decimal(precision: 18, scale: 2),
                        HalfWholeSalePrice = c.Decimal(precision: 18, scale: 2),
                        RetailPrice = c.Decimal(precision: 18, scale: 2),
                        SalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QTY = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CompanyId = c.Int(),
                        SupplierId = c.Int(),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .Index(t => t.CompanyId)
                .Index(t => t.SupplierId)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Balance = c.Decimal(precision: 18, scale: 2),
                        Site = c.String(),
                        Image = c.Binary(storeType: "image"),
                        Email = c.String(),
                        Phone = c.String(),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .Index(t => t.Name, unique: true)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Site = c.String(),
                        Image = c.Binary(storeType: "image"),
                        Email = c.String(),
                        Phone = c.String(),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .Index(t => t.Name, unique: true)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Site = c.String(),
                        Image = c.Binary(storeType: "image"),
                        Email = c.String(),
                        Phone = c.String(),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .Index(t => t.Name, unique: true)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Balance = c.Decimal(precision: 18, scale: 2),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .Index(t => t.Name, unique: true)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Bills", "EditById", "dbo.Users");
            DropForeignKey("dbo.Bills", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Bills", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Bills", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Clients", "EditById", "dbo.Users");
            DropForeignKey("dbo.Clients", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.BillMoves", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Services", "EditById", "dbo.Users");
            DropForeignKey("dbo.Services", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.BillMoves", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.Suppliers", "EditById", "dbo.Users");
            DropForeignKey("dbo.Suppliers", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Items", "EditById", "dbo.Users");
            DropForeignKey("dbo.Items", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Items", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Companies", "EditById", "dbo.Users");
            DropForeignKey("dbo.Companies", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.BillMoves", "EditById", "dbo.Users");
            DropForeignKey("dbo.BillMoves", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.BillMoves", "BillId", "dbo.Bills");
            DropIndex("dbo.Clients", new[] { "EditById" });
            DropIndex("dbo.Clients", new[] { "CreatedById" });
            DropIndex("dbo.Clients", new[] { "Name" });
            DropIndex("dbo.Services", new[] { "EditById" });
            DropIndex("dbo.Services", new[] { "CreatedById" });
            DropIndex("dbo.Services", new[] { "Name" });
            DropIndex("dbo.Suppliers", new[] { "EditById" });
            DropIndex("dbo.Suppliers", new[] { "CreatedById" });
            DropIndex("dbo.Suppliers", new[] { "Name" });
            DropIndex("dbo.Companies", new[] { "EditById" });
            DropIndex("dbo.Companies", new[] { "CreatedById" });
            DropIndex("dbo.Companies", new[] { "Name" });
            DropIndex("dbo.Items", new[] { "EditById" });
            DropIndex("dbo.Items", new[] { "CreatedById" });
            DropIndex("dbo.Items", new[] { "SupplierId" });
            DropIndex("dbo.Items", new[] { "CompanyId" });
            DropIndex("dbo.Users", new[] { "Name" });
            DropIndex("dbo.BillMoves", new[] { "EditById" });
            DropIndex("dbo.BillMoves", new[] { "CreatedById" });
            DropIndex("dbo.BillMoves", new[] { "ServiceId" });
            DropIndex("dbo.BillMoves", new[] { "ItemId" });
            DropIndex("dbo.BillMoves", new[] { "BillId" });
            DropIndex("dbo.Bills", new[] { "EditById" });
            DropIndex("dbo.Bills", new[] { "CreatedById" });
            DropIndex("dbo.Bills", new[] { "ServiceId" });
            DropIndex("dbo.Bills", new[] { "CompanyId" });
            DropIndex("dbo.Bills", new[] { "ClientId" });
            DropTable("dbo.Clients");
            DropTable("dbo.Services");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Companies");
            DropTable("dbo.Items");
            DropTable("dbo.Users");
            DropTable("dbo.BillMoves");
            DropTable("dbo.Bills");
        }
    }
}
