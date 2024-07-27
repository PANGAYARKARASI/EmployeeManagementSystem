using EmployeeLibrary.Model;
using EmployeeLibrary.Repo;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLibrary.Repository
{
    public class RoleRepository : IRoleRepository
    {
        EmployeeDbContext dbContext;
        public RoleRepository(EmployeeDbContext roleRepo)
        {
            dbContext = roleRepo;
        }
        public async Task DeleteRole(int roleid)
        {
            try
            {
                Role roletodelete = await GetRoleById(roleid);
                dbContext.Roles.Remove(roletodelete);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Role>> GetAllRoles()
        {
            try
            {
                List<Role> roles = await dbContext.Roles.ToListAsync<Role>();
                return roles;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Role> GetRoleById(int roleid)
        {
            try
            {
                Role role = await (from u in dbContext.Roles where u.RoleId == roleid select u).FirstAsync();
                return role;
            }
            catch (Exception)
            {
                throw new Exception("Role Id not Exists");
            }
        }
        public async Task InsertRole(Role role)
        {
            try
            {
                await dbContext.Roles.AddAsync(role);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateRole(int roleid, Role role)
        {
            try
            {
                Role roleupdate = await GetRoleById(roleid);
                roleupdate.RoleName = role.RoleName;
                roleupdate.RoleSalary = role.RoleSalary;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
