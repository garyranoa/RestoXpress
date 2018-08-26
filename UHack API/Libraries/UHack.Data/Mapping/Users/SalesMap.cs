using UHack.Core.Domain.Users;

namespace UHack.Data.Mapping.Users
{
    public partial class SalesMap : AppEntityTypeConfiguration<Sales>
    {
        public SalesMap()
        {
            this.ToTable("sales");
            this.HasKey(b => b.Id);
        }

    }
}
