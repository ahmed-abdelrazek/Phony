namespace Phony.Migrations
{
    using System;
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
            Sql($"SET IDENTITY_INSERT [dbo].[Users] ON; INSERT [dbo].[Users] ([Id], [Name], [Pass], [Group], [Phone], [Notes], [IsActive]) VALUES (1, N'admin', N'$MYHASH$V1$10000$xQaTtKU6U52if7+pq1NsSb+tTSvIrB7BVez88O5qKydHSgFP', 1, NULL, NULL, 1); SET IDENTITY_INSERT [dbo].[Users] OFF");
            Sql($"SET IDENTITY_INSERT [dbo].[Clients] ON; INSERT [dbo].[Clients] ([Id], [Name], [Balance], [Site], [Email], [Phone], [Notes], [CreatedById], [CreateDate], [EditById], [EditDate]) VALUES (1, N'كاش', CAST(0.00 AS Decimal(18, 2)), NULL, NULL, NULL, NULL, 1, GETDATE(), NULL, NULL); SET IDENTITY_INSERT [dbo].[Clients] OFF");
            Sql($"SET IDENTITY_INSERT [dbo].[Companies] ON; INSERT [dbo].[Companies] ([Id], [Name], [Balance], [Image], [Site], [Email], [Phone], [Notes], [CreatedById], [CreateDate], [EditById], [EditDate]) VALUES (1, N'لا يوجد', CAST(0.00 AS Decimal(18, 2)), NULL, NULL, NULL, NULL, NULL, 1, GETDATE(), NULL, NULL); SET IDENTITY_INSERT [dbo].[Companies] OFF");
            Sql($"SET IDENTITY_INSERT [dbo].[SalesMen] ON; INSERT [dbo].[SalesMen] ([Id], [Name], [Balance], [Site], [Email], [Phone], [Notes], [CreatedById], [CreateDate], [EditById], [EditDate]) VALUES (1, N'لا يوجد', CAST(0.00 AS Decimal(18, 2)), NULL, NULL, NULL, NULL, 1, GETDATE(), NULL, NULL); SET IDENTITY_INSERT [dbo].[SalesMen] OFF");
            Sql($"SET IDENTITY_INSERT [dbo].[Stores] ON; INSERT [dbo].[Stores] ([Id], [Name], [Image], [Address1], [Address2], [Tel1], [Tel2], [Phone1], [Phone2], [Email1], [Email2], [Site], [Notes], [CreatedById], [CreateDate], [EditById], [EditDate], [Motto]) VALUES (1, N'التوكل', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, GETDATE(), NULL, NULL, NULL); SET IDENTITY_INSERT [dbo].[Stores] OFF");
            Sql($"SET IDENTITY_INSERT [dbo].[Suppliers] ON; INSERT [dbo].[Suppliers] ([Id], [Name], [Balance], [Image], [Site], [Email], [Phone], [SalesManId], [Notes], [CreatedById], [CreateDate], [EditById], [EditDate]) VALUES (1, N'لا يوجد', CAST(0.00 AS Decimal(18, 2)), NULL, NULL, NULL, NULL, 1, NULL, 1, GETDATE(), NULL, NULL); SET IDENTITY_INSERT [dbo].[Suppliers] OFF");
            Sql($"SET IDENTITY_INSERT [dbo].[Treasuries] ON; INSERT [dbo].[Treasuries] ([Id], [Name], [Balance], [StoreId], [Notes], [CreatedById], [CreateDate], [EditById], [EditDate]) VALUES (1, N'الرئيسية', CAST(0.00 AS Decimal(18, 2)), 1, NULL, 1, GETDATE(), NULL, NULL); SET IDENTITY_INSERT [dbo].[Treasuries] OFF");
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
