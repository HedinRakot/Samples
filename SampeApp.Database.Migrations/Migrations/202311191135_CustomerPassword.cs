using FluentMigrator;

namespace SampeApp.Database.Migrations.Migrations;

[Migration(202311191135, "Customer password")]
public class CustomerPassword : Migration
{
    public override void Up()
    {
        Alter.Table("Customer")
            .AddColumn("Password").AsString(100).NotNullable().WithDefaultValue("123");
    }

    public override void Down()
    {
        Delete.Column("Password").FromTable("Test");
    }
}
