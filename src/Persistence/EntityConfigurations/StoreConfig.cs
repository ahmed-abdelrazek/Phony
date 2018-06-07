using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class StoreConfig : EntityTypeConfiguration<Model.Store>
    {
        public StoreConfig()
        {
            HasKey(s => s.Id);

            Property(s => s.Name)
                .IsRequired();

            HasRequired(s => s.Creator)
                .WithMany()
                .HasForeignKey(s => s.CreatedById)
                .WillCascadeOnDelete(false);

            Property(s => s.EditById)
                .IsOptional();

            HasOptional(s => s.Editor)
                .WithMany()
                .HasForeignKey(s => s.EditById)
                .WillCascadeOnDelete(false);
        }
    }
}