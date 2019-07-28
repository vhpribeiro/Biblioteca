using System;

namespace Biblioteca.Dominio.Permissionamento
{
    public interface IApplicationUser
    {
        string Id { get; set; }
        int AccessFailedCount { get; set; }
        string ConcurrencyStamp { get; set; }
        string Email { get; set; }
        bool EmailConfirmed { get; set; }
        bool LockoutEnabled { get; set; }
        DateTime LockoutEnd { get; set; }
        string NormalizedEmail { get; set; }
        string NormalizedUserName { get; set; }
        string PasswordHash { get; set; }
        string PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }
        string SecurityStamp { get; set; }
        bool TwoFactorEnabled { get; set; }
        string UserName { get; set; }
    }
}