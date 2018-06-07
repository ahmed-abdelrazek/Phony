using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class ServiceMoveConfig : EntityTypeConfiguration<Model.ServiceMove>
    {
        public ServiceMoveConfig()
        {
            HasKey(s => s.Id);

            HasRequired(s => s.Service)
                .WithMany(s=> s.ServicesMoves)
                .HasForeignKey(s => s.ServiceId)
                .WillCascadeOnDelete(false);

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