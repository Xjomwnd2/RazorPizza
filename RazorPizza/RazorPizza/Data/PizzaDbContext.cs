
protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    builder.Entity<ApplicationUser>(entity =>
    {
        entity.Property(e => e.Id)
              .HasColumnType("nvarchar(450)");
    });
}
