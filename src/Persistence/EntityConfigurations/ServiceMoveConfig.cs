using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class ServiceMoveConfig : EntityTypeConfiguration<Model.ServiceMove>
    {
        public ServiceMoveConfig()
        {
            HasKey(c => c.Id);

            HasRequired(c => c.Service)
                .WithMany()
                .HasForeignKey(c => c.ServiceId)
                .WillCascadeOnDelete(false);

            HasRequired(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.CreatedById)
                .WillCascadeOnDelete(false);

            Property(c => c.EditById)
                .IsOptional();

            HasOptional(c => c.Editor)
                .WithMany()
                .HasForeignKey(s => s.EditById)
                .WillCascadeOnDelete(false);
        }
    }
}