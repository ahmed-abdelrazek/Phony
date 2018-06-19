namespace Phony.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class WillCascadeOnDeleteToTrueForMoves : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyMoves", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.SupplierMoves", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.SalesManMoves", "SalesManId", "dbo.SalesMen");
            DropForeignKey("dbo.ServiceMoves", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.ClientMoves", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.TreasuryMoves", "TreasuryId", "dbo.Treasuries");
            AddForeignKey("dbo.CompanyMoves", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SupplierMoves", "SupplierId", "dbo.Suppliers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SalesManMoves", "SalesManId", "dbo.SalesMen", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ServiceMoves", "ServiceId", "dbo.Services", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ClientMoves", "ClientId", "dbo.Clients", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TreasuryMoves", "TreasuryId", "dbo.Treasuries", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TreasuryMoves", "TreasuryId", "dbo.Treasuries");
            DropForeignKey("dbo.ClientMoves", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ServiceMoves", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.SalesManMoves", "SalesManId", "dbo.SalesMen");
            DropForeignKey("dbo.SupplierMoves", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.CompanyMoves", "CompanyId", "dbo.Companies");
            AddForeignKey("dbo.TreasuryMoves", "TreasuryId", "dbo.Treasuries", "Id");
            AddForeignKey("dbo.ClientMoves", "ClientId", "dbo.Clients", "Id");
            AddForeignKey("dbo.ServiceMoves", "ServiceId", "dbo.Services", "Id");
            AddForeignKey("dbo.SalesManMoves", "SalesManId", "dbo.SalesMen", "Id");
            AddForeignKey("dbo.SupplierMoves", "SupplierId", "dbo.Suppliers", "Id");
            AddForeignKey("dbo.CompanyMoves", "CompanyId", "dbo.Companies", "Id");
        }
    }
}
