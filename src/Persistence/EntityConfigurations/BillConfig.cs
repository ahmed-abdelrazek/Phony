using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class BillConfig : EntityTypeConfiguration<Model.Bill>
    {
        public BillConfig()
        {
            HasKey(b => b.Id);

            HasOptional(b => b.Client)
                    .WithMany(c => c.Bills)
                    .HasForeignKey(b => b.ClientId);

            HasOptional(b => b.Company)
                    .WithMany(c => c.Bills)
                    .HasForeignKey(b => b.CompanyId);

            HasOptional(b => b.Service)
                    .WithMany(c => c.Bills)
                    .HasForeignKey(b => b.ServiceId);

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
