using Biblioteca.Infra.Configuracoes.Orm.Permissionamento;
using FluentNHibernate.Mapping;

namespace Biblioteca.Infra.Mapeamento.Permissionamento
{
    public class ApplicationRoleMap : ClassMap<ApplicationRole>
    {
        public ApplicationRoleMap()
        {
            Id(a => a.Id).Not.Nullable();
            Map(a => a.ConcurrencyStamp).Nullable();
            Map(a => a.Name).Nullable();
            Map(a => a.NormalizedName).Nullable();
        }
    }
}