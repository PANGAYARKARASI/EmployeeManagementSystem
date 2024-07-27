
using EmployeeLibrary.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLibrary.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        EmployeeDbContext dbContext;
        public UserRoleRepository(EmployeeDbContext userroleRepo)
        {
            dbContext = userroleRepo;
        }
        public async Task DeleteUserRole(int userroleid)
        {
            try
            {
                UserRole userroletodelete = await GetUserRoleById(userroleid);
                dbContext.UserRoles.Remove(userroletodelete);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<UserRole>> GetAllUserRoles()
        {
            try
            {
                List<UserRole> userroles = await dbContext.UserRoles.Include(role => role.User).Include(role => role.Role).ToListAsync();
                return userroles;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<UserRole>> GetByRole(int roleid)
        {
            try
            {
                List<UserRole> userrole = await (from u in dbContext.UserRoles.Include(role => role.User).Include(role => role.Role) where u.RoleId == roleid select u).ToListAsync();
                return userrole;
            }
            catch (Exception)
            {
                throw new Exception("Role Id not Exists");
            }
        }
        public async Task<List<UserRole>> GetByUser(int userid)
        {
            try
            {
                List<UserRole> userrole = await (from u in dbContext.UserRoles.Include(role => role.User).Include(role => role.Role) where u.UserId == userid select u).ToListAsync();
                return userrole;
            }
            catch (Exception)
            {
                throw new Exception("User Id not Exists");
            }
        }
        public async Task<UserRole> GetUserRoleById(int userroleid)
        {
            try
            {
                UserRole userrole = await (from u in dbContext.UserRoles.Include(role => role.User).Include(role => role.Role) where u.UserRoleId == userroleid select u).FirstAsync();
                return userrole;
            }
            catch (Exception)
            {
                throw new Exception("User Id not Exists");
            }
        }
        public async Task InsertUserRole(UserRole userrole)
        {
            try
            {
                await dbContext.UserRoles.AddAsync(userrole);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateUserRole(int userroleid, UserRole userrole)
        {
            try
            {
                UserRole userroletoup = await GetUserRoleById(userroleid);
                //userroletoup.UserId = userrole.UserId;
                userroletoup.RoleId = userrole.RoleId;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
