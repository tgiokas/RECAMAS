using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class AppSettingsConfiguration : IEntityTypeConfiguration<AppSettings>
{
    public void Configure(EntityTypeBuilder<AppSettings> builder)
    {
        builder.ToTable("app_settings", schema: "admin");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.SettingKey).HasComment("The key of the setting or parameter");
        builder.Property(e => e.SettingValue).HasComment("The value of the setting — string");
        builder.Property(e => e.DisplayName).HasComment("Το όνομα της παραμέτρου όπως αυτή θα εμφανίζεται στο UI");
        builder.Property(e => e.Description).HasComment("Σύντομο κείμενο που να εξηγεί την παράμετρο ή την ρύθμιση");
        builder.Property(e => e.Data_Type).HasComment("Ο τύπος της τιμής SettingValue");
        builder.Property(e => e.Category).HasComment("Ο τύπος της τιμής SettingValue");
        builder.HasIndex(e => e.SettingKey).IsUnique();
    }
}
