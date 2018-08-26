using UHack.Core.Domain.Users;

namespace UHack.Data.Mapping.Users
{
    public partial class ProductMap : AppEntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            this.ToTable("products");
            this.HasKey(b => b.Id);
        }

    }
}
