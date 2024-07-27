using EmployeeLibrary.Model;


namespace EmployeeLibrary.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUsersById(int userid);
        Task<User> GetByEmail(string email);
        Task InsertUser(User user);
        Task UpdateUser(int userid, User user);
        Task DeleteUser(int userid);
    }
}