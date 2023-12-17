using FluentMigrator;
using System.IO;

namespace SampeApp.Database.Migrations.Migrations;

[Migration(202312171024, "Coupon")]
public class Coupon : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("Coupon")
        .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
        .WithColumn("Discount").AsInt32().NotNullable()
        .WithColumn("Count").AsInt32().Nullable()
        .WithColumn("AppliedCount").AsInt32().Nullable()
        .WithColumn("Version").AsTime().NotNullable();
    }
}
