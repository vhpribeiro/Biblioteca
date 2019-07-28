using Biblioteca.Infra.Configuracoes.Orm.Permissionamento;
using FluentNHibernate.Mapping;

namespace Biblioteca.Infra.Mapeamento.Permissionamento
{
    public class ApplicationUserMap : ClassMap<ApplicationUser>
    {
        public ApplicationUserMap()
        {
            Id(a => a.Id).Not.Nullable();
            Map(a => a.AccessFailedCount).Not.Nullable();
            Map(a => a.ConcurrencyStamp).Nullable();
            Map(a => a.Email).Nullable();
            Map(a => a.EmailConfirmed).Nullable();
            Map(a => a.LockoutEnabled).Nullable();
            Map(a => a.LockoutEnd).Nullable();
            Map(a => a.NormalizedEmail).Nullable();
            Map(a => a.NormalizedUserName).Nullable();
            Map(a => a.PasswordHash).Nullable();
            Map(a => a.PhoneNumber).Nullable();
            Map(a => a.PhoneNumberConfirmed).Nullable();
            Map(a => a.SecurityStamp).Nullable();
            Map(a => a.TwoFactorEnabled).Nullable();
            Map(a => a.UserName).Nullable();
        }
    }
}