namespace Phony.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class NewMovesModels1 : DbMigration
    {
        public override void Up()
        {
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
                        Name = c.String(nullable: false),
                        Group = c.Byte(nullable: false),
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
            DropForeignKey("dbo.ClientMoves", "EditById", "dbo.Users");
            DropForeignKey("dbo.ClientMoves", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClientMoves", "ClientId", "dbo.Clients");
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
            DropIndex("dbo.ClientMoves", new[] { "EditById" });
            DropIndex("dbo.ClientMoves", new[] { "CreatedById" });
            DropIndex("dbo.ClientMoves", new[] { "ClientId" });
            DropTable("dbo.SupplierMoves");
            DropTable("dbo.ServiceMoves");
            DropTable("dbo.Notes");
            DropTable("dbo.CompanyMoves");
            DropTable("dbo.ClientMoves");
        }
    }
}
