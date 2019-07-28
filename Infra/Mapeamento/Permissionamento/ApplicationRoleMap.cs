using Biblioteca.Infra.Configuracoes.Orm.Permissionamento;
using FluentNHibernate.Mapping;

namespace Biblioteca.Infra.Mapeamento.Permissionamento
{
    public class ApplicationRoleMap : ClassMap<ApplicationRole>
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