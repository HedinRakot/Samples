using FluentMigrator;
using System.IO;

namespace SampeApp.Database.Migrations.Migrations;

[Migration(202311051100, "Initial")]
public class Initial : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("Customer")
        .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
        .WithColumn("Name").AsString(10).NotNullable()
        .WithColumn("LastName").AsString(50).NotNullable()
        .WithColumn("Email").AsString(100).NotNullable()
        .WithColumn("Number").AsInt32().NotNullable()
        .WithColumn("CustomValidationField").AsString(100).NotNullable()
        .WithColumn("PhotoString").AsString(Int32.MaxValue).Nullable()
        .WithColumn("PhotoBinary").AsBinary(Int32.MaxValue).Nullable()
        .WithColumn("Password").AsString(100).NotNullable();

        Create.Table("Address")
        .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
        .WithColumn("Street").AsString(50).NotNullable()
        .WithColumn("City").AsString(50).NotNullable()
        .WithColumn("Housenumber").AsString(50).NotNullable()
        .WithColumn("Type").AsInt32().NotNullable();

        Create.Table("CustomersAddress")
       .WithColumn("CustomersId").AsInt64().NotNullable().ForeignKey("Customer", "Id")
       .WithColumn("AddressId").AsInt64().NotNullable().ForeignKey("Address", "Id");

        Create.Table("Order")
          .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
          .WithColumn("OrderNumber").AsString(50).NotNullable()
          .WithColumn("CustomerId").AsInt64().NotNullable().ForeignKey("Customer", "Id");

        Create.Table("OrderHistory")
        .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
        .WithColumn("OrderId").AsInt64().NotNullable().ForeignKey("Order", "Id")
        .WithColumn("Changes").AsString(150).NotNullable()
        .WithColumn("ChangeDate").AsDateTimeOffset().NotNullable();
    }
}
