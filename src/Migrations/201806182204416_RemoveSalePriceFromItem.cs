namespace Phony.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RemoveSalePriceFromItem : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE dbo.Items SET RetailPrice = SalePrice");
            DropColumn("dbo.Items", "SalePrice");
        }
        
        public override void Down()
        {
            Sql("UPDATE dbo.Items SET SalePrice = RetailPrice");
            AddColumn("dbo.Items", "SalePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
