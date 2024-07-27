using EmployeeLibrary.Model;

namespace EmployeeLibrary.Repo
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllRoles();
        Task<Role> GetRoleById(int roleid);
        Task InsertRole(Role role);
        Task UpdateRole(int roleid, Role role);
        Task DeleteRole(int roleid);
    }
}
