using UHack.Core.Domain.Configuration;
using UHack.Data.Mapping;

namespace UHack.Data.Configuration
{
    public partial class SettingMap : AppEntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            this.ToTable("settings");
            this.HasKey(s => s.Id);
            this.Property(s => s.Name).IsRequired().HasMaxLength(200);
            this.Property(s => s.Value).IsRequired().HasMaxLength(200);
        }
    }
}
