using FluentNHibernate.Mapping;
using Microsoft.AspNetCore.Identity;

namespace Biblioteca.Infra.Mapeamento.Permissionamento
{
    public class ApplicationRoleMap : ClassMap<IdentityRole>
    {
        public ApplicationRoleMap()
        {
            Id(a => a.Id);
            Map(a => a.ConcurrencyStamp);
            Map(a => a.Name);
            Map(a => a.NormalizedName);
        }
    }
}