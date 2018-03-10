using System.Data.Entity.ModelConfiguration;

namespace Phony.EntityConfigurations
{
    public class UserConfig : EntityTypeConfiguration<Model.User>
    {
        public UserConfig()
        {
            HasKey(u => u.Id);

            Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);

            HasIndex(u => u.Name)
                .IsUnique();
        }
    }
}
