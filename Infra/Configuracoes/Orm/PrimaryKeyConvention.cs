﻿using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Biblioteca.Infra.Configuracoes.Orm
{
    public class PrimaryKeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Column("Id");
        }
    }
}