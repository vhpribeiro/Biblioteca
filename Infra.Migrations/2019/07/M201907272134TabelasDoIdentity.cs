using FluentMigrator;

namespace Biblioteca.Infra.Migrations._2019._07
{
    [Migration(201907272134)]
    public class M201907272134TabelasDoIdentity : Migration
    {
        public override void Up()
        {
            Create.Table("AspNetRoles")
                .WithColumn("Id").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("ConcurrencyStamp").AsString().Nullable()
                .WithColumn("Name").AsString(256).Nullable()
                .WithColumn("NormalizedName").AsString(256).Nullable();

            Create.Table("AspNetUserTokens")
                .WithColumn("UserId").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("LoginProvider").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("Name").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("Value").AsString().Nullable();

            Create.Table("AspNetUsers")
                .WithColumn("Id").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("AccessFailedCount").AsInt32().NotNullable()
                .WithColumn("ConcurrencyStamp").AsString(450).Nullable()
                .WithColumn("Email").AsString(256).Nullable()
                .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
                .WithColumn("LockoutEnabled").AsBoolean().NotNullable()
                .WithColumn("LockoutEnd").AsDateTime().Nullable()
                .WithColumn("NormalizedEmail").AsString(256).Nullable()
                .WithColumn("NormalizedUserName").AsString(256).Nullable()
                .WithColumn("PasswordHash").AsString(450).Nullable()
                .WithColumn("PhoneNumber").AsString(450).Nullable()
                .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable()
                .WithColumn("SecurityStamp").AsString(450).Nullable()
                .WithColumn("TwoFactorEnabled").AsBoolean().NotNullable()
                .WithColumn("UserName").AsString(256).Nullable();

            Create.Table("AspNetRoleClaims")
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("ClaimType").AsString().Nullable()
                .WithColumn("ClaimValue").AsString().Nullable()
                .WithColumn("RoleId").AsString(450).NotNullable();
            Create.ForeignKey().FromTable("AspNetRoleClaims").ForeignColumn("RoleId").ToTable("AspNetRoles").PrimaryColumn("Id");

            Create.Table("AspNetUserClaims")
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("ClaimType").AsString().Nullable()
                .WithColumn("ClaimValue").AsString().Nullable()
                .WithColumn("UserId").AsString(450).NotNullable();
            Create.ForeignKey().FromTable("AspNetUserClaims").ForeignColumn("UserId").ToTable("AspNetUsers").PrimaryColumn("Id");

            Create.Table("AspNetUserLogins")
                .WithColumn("LoginProvider").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("ProviderKey").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("ProviderDisplayName").AsString().Nullable()
                .WithColumn("UserId").AsString(450).NotNullable();
            Create.ForeignKey().FromTable("AspNetUserLogins").ForeignColumn("UserId").ToTable("AspNetUsers").PrimaryColumn("Id");

            Create.Table("AspNetUserRoles")
                .WithColumn("UserId").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("RoleId").AsString(450).PrimaryKey().NotNullable();
            Create.ForeignKey().FromTable("AspNetUserRoles").ForeignColumn("UserId").ToTable("AspNetUsers").PrimaryColumn("Id");
            Create.ForeignKey().FromTable("AspNetUserRoles").ForeignColumn("RoleId").ToTable("AspNetRoles").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("AspNetRoles");
            Delete.Table("AspNetUserTokens");
            Delete.Table("AspNetUsers");
            Delete.Table("AspNetRoleClaims");
            Delete.Table("AspNetUserClaims");
            Delete.Table("AspNetUserLogins");
            Delete.Table("AspNetUserRoles");
        }
    }
}