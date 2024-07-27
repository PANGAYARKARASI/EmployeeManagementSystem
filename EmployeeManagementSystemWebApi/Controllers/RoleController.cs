using EmployeeLibrary.Model;
using EmployeeLibrary.Repo;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        IRoleRepository repo;
        public RoleController(IRoleRepository roleRepo)
        {
            repo = roleRepo;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllRoleDetails()
        {
            List<Role> roles = await repo.GetAllRoles();
            return Ok(roles);
        }
        [HttpGet("ByRoleId/{roleid}")]
        public async Task<ActionResult> GetOne(int roleid)
        {
            try
            {
                Role role = await repo.GetRoleById(roleid);
                return Ok(role);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Insert(Role role)
        {
            try
            {
                await repo.InsertRole(role);
                return Created($"api/Role/{role.RoleId}", role);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpPut("{roleid}")]
        public async Task<ActionResult> Update(int roleid, Role role)
        {
            try
            {
                await repo.UpdateRole(roleid, role);
                return Ok(role);
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }
        [HttpDelete("{roleid}")]
        public async Task<ActionResult> Delete(int roleid)
        {
            try
            {
                await repo.DeleteRole(roleid);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
