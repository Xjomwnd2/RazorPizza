protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Order entity
    modelBuilder.Entity<Order>(entity =>
    {
        entity.Property(e => e.SubTotal).HasPrecision(18, 2);
        entity.Property(e => e.Tax).HasPrecision(18, 2);
        entity.Property(e => e.DeliveryFee).HasPrecision(18, 2);
        entity.Property(e => e.Discount).HasPrecision(18, 2);
        entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
    });

    // OrderItem entity
    modelBuilder.Entity<OrderItem>(entity =>
    {
        entity.Property(e => e.Price).HasPrecision(18, 2);
    });

    // Pizza entity
    modelBuilder.Entity<Pizza>(entity =>
    {
        entity.Property(e => e.BasePrice).HasPrecision(18, 2);
    });

    // PromoCode entity
    modelBuilder.Entity<PromoCode>(entity =>
    {
        entity.Property(e => e.DiscountPercent).HasPrecision(5, 2);
        entity.Property(e => e.DiscountValue).HasPrecision(18, 2);
        entity.Property(e => e.MaxDiscountAmount).HasPrecision(18, 2);
        entity.Property(e => e.MinOrderAmount).HasPrecision(18, 2);
    });

    // Topping entity
    modelBuilder.Entity<Topping>(entity =>
    {
        entity.Property(e => e.Price).HasPrecision(18, 2);
    });
}