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
                        ClientId = c.Int(nullable: false),
                        StoreId = c.Int(nullable: false),
                        Discount = c.Decimal(precision: 18, scale: 2),
                        TotalAfterDiscounts = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPayed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                        BillMove_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BillMoves", t => t.BillMove_Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .ForeignKey("dbo.Stores", t => t.StoreId)
                .Index(t => t.ClientId)
                .Index(t => t.StoreId)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById)
                .Index(t => t.BillMove_Id);
            
            CreateTable(
                "dbo.BillMoves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BillId = c.Int(nullable: false),
                        ItemId = c.Int(),
                        QTY = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ServiceId = c.Int(),
                        Discount = c.Decimal(precision: 18, scale: 2),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bills", t => t.BillId, cascadeDelete: true)
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
                        Pass = c.String(nullable: false, maxLength: 100),
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
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Image = c.Binary(storeType: "image"),
                        Site = c.String(),
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
                        Image = c.Binary(storeType: "image"),
                        Site = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        SalesManId = c.Int(nullable: false),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .ForeignKey("dbo.SalesMen", t => t.SalesManId)
                .Index(t => t.Name, unique: true)
                .Index(t => t.SalesManId)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
            CreateTable(
                "dbo.SalesMen",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Site = c.String(),
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
                "dbo.SalesManMoves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SalesManId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .ForeignKey("dbo.SalesMen", t => t.SalesManId)
                .Index(t => t.SalesManId)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Image = c.Binary(storeType: "image"),
                        Site = c.String(),
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
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Site = c.String(),
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
                "dbo.ClientMoves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .Index(t => t.ClientId)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Image = c.Binary(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        Tel1 = c.String(),
                        Tel2 = c.String(),
                        Phone1 = c.String(),
                        Phone2 = c.String(),
                        Email1 = c.String(),
                        Email2 = c.String(),
                        Site = c.String(),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
            CreateTable(
                "dbo.CompanyMoves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                .Index(t => t.CompanyId)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Group = c.Byte(nullable: false),
                        Phone = c.String(),
                        Notes = c.String(nullable: false),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
            CreateTable(
                "dbo.ServiceMoves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .ForeignKey("dbo.Services", t => t.ServiceId)
                .Index(t => t.ServiceId)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
            CreateTable(
                "dbo.SupplierMoves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SupplierId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EditById = c.Int(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.EditById)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .Index(t => t.SupplierId)
                .Index(t => t.CreatedById)
                .Index(t => t.EditById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupplierMoves", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.SupplierMoves", "EditById", "dbo.Users");
            DropForeignKey("dbo.SupplierMoves", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ServiceMoves", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.ServiceMoves", "EditById", "dbo.Users");
            DropForeignKey("dbo.ServiceMoves", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Notes", "EditById", "dbo.Users");
            DropForeignKey("dbo.Notes", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.CompanyMoves", "EditById", "dbo.Users");
            DropForeignKey("dbo.CompanyMoves", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.CompanyMoves", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Bills", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.Stores", "EditById", "dbo.Users");
            DropForeignKey("dbo.Stores", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Bills", "EditById", "dbo.Users");
            DropForeignKey("dbo.Bills", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Bills", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Clients", "EditById", "dbo.Users");
            DropForeignKey("dbo.Clients", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClientMoves", "EditById", "dbo.Users");
            DropForeignKey("dbo.ClientMoves", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClientMoves", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.BillMoves", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Services", "EditById", "dbo.Users");
            DropForeignKey("dbo.Services", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.BillMoves", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.Suppliers", "SalesManId", "dbo.SalesMen");
            DropForeignKey("dbo.SalesManMoves", "SalesManId", "dbo.SalesMen");
            DropForeignKey("dbo.SalesManMoves", "EditById", "dbo.Users");
            DropForeignKey("dbo.SalesManMoves", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SalesMen", "EditById", "dbo.Users");
            DropForeignKey("dbo.SalesMen", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Suppliers", "EditById", "dbo.Users");
            DropForeignKey("dbo.Suppliers", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Items", "EditById", "dbo.Users");
            DropForeignKey("dbo.Items", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Items", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Companies", "EditById", "dbo.Users");
            DropForeignKey("dbo.Companies", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.BillMoves", "EditById", "dbo.Users");
            DropForeignKey("dbo.BillMoves", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Bills", "BillMove_Id", "dbo.BillMoves");
            DropForeignKey("dbo.BillMoves", "BillId", "dbo.Bills");
            DropIndex("dbo.SupplierMoves", new[] { "EditById" });
            DropIndex("dbo.SupplierMoves", new[] { "CreatedById" });
            DropIndex("dbo.SupplierMoves", new[] { "SupplierId" });
            DropIndex("dbo.ServiceMoves", new[] { "EditById" });
            DropIndex("dbo.ServiceMoves", new[] { "CreatedById" });
            DropIndex("dbo.ServiceMoves", new[] { "ServiceId" });
            DropIndex("dbo.Notes", new[] { "EditById" });
            DropIndex("dbo.Notes", new[] { "CreatedById" });
            DropIndex("dbo.CompanyMoves", new[] { "EditById" });
            DropIndex("dbo.CompanyMoves", new[] { "CreatedById" });
            DropIndex("dbo.CompanyMoves", new[] { "CompanyId" });
            DropIndex("dbo.Stores", new[] { "EditById" });
            DropIndex("dbo.Stores", new[] { "CreatedById" });
            DropIndex("dbo.ClientMoves", new[] { "EditById" });
            DropIndex("dbo.ClientMoves", new[] { "CreatedById" });
            DropIndex("dbo.ClientMoves", new[] { "ClientId" });
            DropIndex("dbo.Clients", new[] { "EditById" });
            DropIndex("dbo.Clients", new[] { "CreatedById" });
            DropIndex("dbo.Clients", new[] { "Name" });
            DropIndex("dbo.Services", new[] { "EditById" });
            DropIndex("dbo.Services", new[] { "CreatedById" });
            DropIndex("dbo.Services", new[] { "Name" });
            DropIndex("dbo.SalesManMoves", new[] { "EditById" });
            DropIndex("dbo.SalesManMoves", new[] { "CreatedById" });
            DropIndex("dbo.SalesManMoves", new[] { "SalesManId" });
            DropIndex("dbo.SalesMen", new[] { "EditById" });
            DropIndex("dbo.SalesMen", new[] { "CreatedById" });
            DropIndex("dbo.SalesMen", new[] { "Name" });
            DropIndex("dbo.Suppliers", new[] { "EditById" });
            DropIndex("dbo.Suppliers", new[] { "CreatedById" });
            DropIndex("dbo.Suppliers", new[] { "SalesManId" });
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
            DropIndex("dbo.Bills", new[] { "BillMove_Id" });
            DropIndex("dbo.Bills", new[] { "EditById" });
            DropIndex("dbo.Bills", new[] { "CreatedById" });
            DropIndex("dbo.Bills", new[] { "StoreId" });
            DropIndex("dbo.Bills", new[] { "ClientId" });
            DropTable("dbo.SupplierMoves");
            DropTable("dbo.ServiceMoves");
            DropTable("dbo.Notes");
            DropTable("dbo.CompanyMoves");
            DropTable("dbo.Stores");
            DropTable("dbo.ClientMoves");
            DropTable("dbo.Clients");
            DropTable("dbo.Services");
            DropTable("dbo.SalesManMoves");
            DropTable("dbo.SalesMen");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Companies");
            DropTable("dbo.Items");
            DropTable("dbo.Users");
            DropTable("dbo.BillMoves");
            DropTable("dbo.Bills");
        }
    }
}
