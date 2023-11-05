using FluentMigrator;

namespace SampeApp.Database.Migrations.Migrations;

[Migration(202311051146, "Add Test Table")]
public class AddTestTable : Migration
{
    public override void Up()
    {
        Create.Table("Test")
            .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
            .WithColumn("Text").AsString(20).Nullable();
    }

    public override void Down()
    {
        Delete.Table("Test");
    }
}
