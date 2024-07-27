using EmployeeLibrary.Model;
using EmployeeLibrary.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        IUserRoleRepository repo;
        public UserRoleController(IUserRoleRepository userroleRepo)
        {
            repo = userroleRepo;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUserRoleDetails()
        {
            List<UserRole> userroles = await repo.GetAllUserRoles();
            return Ok(userroles);
        }
        [HttpGet("ByUserRoleId/{userroleid}")]
        public async Task<ActionResult> GetOne(int userroleid)
        {
            try
            {
                UserRole userrole = await repo.GetUserRoleById(userroleid);
                return Ok(userrole);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("ByUserId/{userid}")]
        public async Task<ActionResult> GetByUser(int userid)
        {
            try
            {
                List<UserRole> userrole = await repo.GetByUser(userid);
                return Ok(userrole);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("ByRoleId/{roleid}")]
        public async Task<ActionResult> GetByRole(int roleid)
        {
            try
            {
                List<UserRole> userrole = await repo.GetByRole(roleid);
                return Ok(userrole);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Insert(UserRole userrole)
        {
            try
            {
                await repo.InsertUserRole(userrole);
                return Created($"api/UserRole/{userrole.UserRoleId}", userrole);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpPut("{userroleid}")]
        public async Task<ActionResult> Update(int userroleid, UserRole userrole)
        {
            try
            {
                await repo.UpdateUserRole(userroleid, userrole);
                return Ok(userrole);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("{userroleid}")]
        public async Task<ActionResult> Delete(int userroleid)
        {
            try
            {
                await repo.DeleteUserRole(userroleid);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
