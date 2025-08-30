using System.Threading.Tasks;

namespace RoleBased.Seed
{
    public interface IRoleSeeder
    {
        Task SeedRolesAndUsersAsync();
    }
}
