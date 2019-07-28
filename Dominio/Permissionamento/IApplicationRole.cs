namespace Biblioteca.Dominio.Permissionamento
{
    public interface IApplicationRole
    {
        string Id { get; set; }
        string ConcurrencyStamp { get; set; }
        string Name { get; set; }
        string NormalizedName { get; set; }
    }
}