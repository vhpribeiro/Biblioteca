﻿using System;
using FluentNHibernate;
using FluentNHibernate.Conventions;

namespace Biblioteca.Infra.Configuracoes.Orm
{
    public class CustomForeignKeyConvention : ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            if (property == null)
                return "Id" + type.Name;
            return "Id" + property.Name;
        }
    }
}