using EmployeeLibrary.Model;
using EmployeeLibrary.Repository;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLibrary.Repo
{
    public class UserRepository : IUserRepository
    {
        EmployeeDbContext dbContext;

        public UserRepository(EmployeeDbContext userRepo)
        {
            dbContext = userRepo;
        }
        public async Task DeleteUser(int userid)
        {
            try
            {
                User usertodelete = await GetUsersById(userid);
                dbContext.Users.Remove(usertodelete);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                List<User> users = await dbContext.Users.ToListAsync<User>();
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<User> GetByEmail(string email)
        {
            try
            {
                User user = await (from u in dbContext.Users where u.Email == email select u).FirstAsync();
                return user;
            }
            catch (Exception)
            {
                throw new Exception("Email Id not Exists");
            }
        }
        public async Task<User> GetUsersById(int userid)
        {
            try
            {
                User user = await (from u in dbContext.Users where u.UserId == userid select u).FirstAsync();
                return user;
            }
            catch (Exception)
            {
                throw new Exception("User Id not Exists");
            }
        }
        public async Task InsertUser(User user)
        {
            try
            {
                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateUser(int userid, User user)
        {
            try
            {
                User usertoup = await GetUsersById(userid);
                usertoup.FirstName = user.FirstName;
                usertoup.LastName = user.LastName;
                usertoup.Password = user.Password;
                usertoup.Email = user.Email;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
