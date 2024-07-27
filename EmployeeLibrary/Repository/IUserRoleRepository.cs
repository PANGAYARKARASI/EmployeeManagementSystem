using EmployeeLibrary.Model;

namespace EmployeeLibrary.Repository
{
    public interface IUserRoleRepository
    {
        Task<List<UserRole>> GetAllUserRoles();
        Task<UserRole> GetUserRoleById(int userroleid);
        Task<List<UserRole>> GetByUser(int userid);
        Task<List<UserRole>> GetByRole(int roleid);
        Task InsertUserRole(UserRole userrole);
        Task UpdateUserRole(int userroleid, UserRole userrole);
        Task DeleteUserRole(int userroleid);
    }
}
