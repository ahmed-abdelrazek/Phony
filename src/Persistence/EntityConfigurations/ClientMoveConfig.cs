using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class ClientMoveConfig : EntityTypeConfiguration<Model.ClientMove>
    {
        public ClientMoveConfig()
        {
            HasKey(c => c.Id);

            HasRequired(c => c.Client)
                .WithMany(c => c.ClientsMoves)
                .HasForeignKey(c => c.ClientId)
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