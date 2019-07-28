using System;
using Microsoft.AspNetCore.Identity;
using NHibernate;
using NHibernate.AspNetCore.Identity;

namespace Biblioteca.Infra.Configuracoes.Orm.Permissionamento
{
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole>
    {
        private ISession session;

        public ApplicationUserStore(ISession session, IdentityErrorDescriber errorDescriber = null)
            : base(session, errorDescriber ?? new IdentityErrorDescriber())
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            this.session = session;
        }
    }
}