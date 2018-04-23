using Phony.Model;
using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class BillConfig : EntityTypeConfiguration<Bill>
    {
        public BillConfig()
        {
            HasKey(b => b.Id);

            Property(b => b.Discount)
                .IsOptional();

            Property(b => b.ClientId)
                .IsOptional();

            Property(b => b.CompanyId)
                .IsOptional();

            HasOptional(b => b.Client)
                .WithMany(c => c.Bills)
                .HasForeignKey(b => b.ClientId)
                .WillCascadeOnDelete(false);

            HasOptional(b => b.Company)
                .WithMany(c => c.Bills)
                .HasForeignKey(b => b.CompanyId)
                .WillCascadeOnDelete(false);

            HasRequired(b => b.Creator)
                .WithMany()
                .HasForeignKey(s => s.CreatedById)
                .WillCascadeOnDelete(false);

            Property(b => b.EditById)
                .IsOptional();

            HasOptional(b => b.Editor)
                .WithMany()
                .HasForeignKey(b => b.EditById)
                .WillCascadeOnDelete(false);
        }
    }
}