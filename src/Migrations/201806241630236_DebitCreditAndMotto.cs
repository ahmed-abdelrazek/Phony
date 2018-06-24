namespace Phony.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class DebitCreditAndMotto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyMoves", "Debit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CompanyMoves", "Credit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SalesManMoves", "Debit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SalesManMoves", "Credit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SupplierMoves", "Debit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SupplierMoves", "Credit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ServiceMoves", "Debit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ServiceMoves", "Credit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ClientMoves", "Debit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ClientMoves", "Credit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Stores", "Motto", c => c.String());
            AddColumn("dbo.TreasuryMoves", "Debit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TreasuryMoves", "Credit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            Sql("UPDATE dbo.CompanyMoves SET Credit = Amount;");
            Sql("UPDATE dbo.SalesManMoves SET Credit = Amount;");
            Sql("UPDATE dbo.SupplierMoves SET Credit = Amount;");
            Sql("UPDATE dbo.ServiceMoves SET Credit = Amount;");
            Sql("UPDATE dbo.ClientMoves SET Debit = Amount;");
            Sql("UPDATE dbo.TreasuryMoves SET Credit = [Out];");
            Sql("UPDATE dbo.TreasuryMoves SET Debit = [In];");
            DropColumn("dbo.CompanyMoves", "Amount");
            DropColumn("dbo.SalesManMoves", "Amount");
            DropColumn("dbo.SupplierMoves", "Amount");
            DropColumn("dbo.ServiceMoves", "Amount");
            DropColumn("dbo.ClientMoves", "Amount");
            DropColumn("dbo.TreasuryMoves", "In");
            DropColumn("dbo.TreasuryMoves", "Out");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreasuryMoves", "Out", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TreasuryMoves", "In", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ClientMoves", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ServiceMoves", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SupplierMoves", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SalesManMoves", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CompanyMoves", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            Sql("UPDATE dbo.CompanyMoves SET Amount = Credit;");
            Sql("UPDATE dbo.SalesManMoves SET Amount = Credit;");
            Sql("UPDATE dbo.SupplierMoves SET Amount = Credit;");
            Sql("UPDATE dbo.ServiceMoves SET Amount = Credit;");
            Sql("UPDATE dbo.ClientMoves SET Amount = Debit;");
            Sql("UPDATE dbo.TreasuryMoves SET [Out] = Credit;");
            Sql("UPDATE dbo.TreasuryMoves SET [In] = Debit;");
            DropColumn("dbo.TreasuryMoves", "Credit");
            DropColumn("dbo.TreasuryMoves", "Debit");
            DropColumn("dbo.Stores", "Motto");
            DropColumn("dbo.ClientMoves", "Credit");
            DropColumn("dbo.ClientMoves", "Debit");
            DropColumn("dbo.ServiceMoves", "Credit");
            DropColumn("dbo.ServiceMoves", "Debit");
            DropColumn("dbo.SupplierMoves", "Credit");
            DropColumn("dbo.SupplierMoves", "Debit");
            DropColumn("dbo.SalesManMoves", "Credit");
            DropColumn("dbo.SalesManMoves", "Debit");
            DropColumn("dbo.CompanyMoves", "Credit");
            DropColumn("dbo.CompanyMoves", "Debit");
        }
    }
}
